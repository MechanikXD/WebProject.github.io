function setupForm() {
    const numEqs = parseInt(document.getElementById('num-eqs').value);
    const numVars = parseInt(document.getElementById('num-vars').value);
    const equationsDiv = document.getElementById('equations');
    equationsDiv.innerHTML = ''; // Clear previous equations

    // Create inputs for each equation
    for (let i = 0; i < numEqs; i++) {
        let equationHtml = `<br></br>`;
        for (let j = 0; j < numVars; j++) {
            equationHtml += `<input type="text" class="coef" data-eq="${i}" data-var="${j}" placeholder="(${i + 1}; ${j + 1})">`;
            if (j < numVars - 1) {
                equationHtml += ' + ';
            }
        }
        equationHtml += ` = <input type="text" class="const" data-eq="${i}" placeholder="b${i + 1}">`;
        equationsDiv.innerHTML += equationHtml;
    }

    // Show the solver form
    document.getElementById('setup-form').style.display = 'none';
    document.getElementById('solver-form').style.display = 'block';
}

function getdMatrixFromHtmL() {
    const matrix = [];
  
    // Select all inputs with the class 'coef' (coefficients for the matrix)
    const coefInputs = document.querySelectorAll('input.coef');
  
    // Organize coefficients into the matrix array
    coefInputs.forEach(input => {
      const eqIndex = parseInt(input.getAttribute('data-eq'));  // Get equation index
      const varIndex = parseInt(input.getAttribute('data-var'));  // Get variable index
      const value = parseFloat(input.value);  // Convert the input value to a number
  
      // Initialize the row if it doesn't exist
      if (!matrix[eqIndex]) {
        matrix[eqIndex] = [];
      }
      
      // Add the coefficient to the appropriate row and column
      matrix[eqIndex][varIndex] = value;
    });
  
    // Select all inputs with the class 'const' (constants on the right side)
    const constInputs = document.querySelectorAll('input.const');
  
    // Append constants to the end of each row in the matrix
    constInputs.forEach(input => {
      const eqIndex = parseInt(input.getAttribute('data-eq'));  // Get equation index
      const value = parseFloat(input.value);  // Convert the input value to a number
  
      // Add the constant as the last column in the row
      matrix[eqIndex].push(value);
    });
  
    console.log('Augmented Matrix:', matrix);
    
    return matrix;
}

async function sendMatrixAndGetId(matrix) {
    loadBalancerUrl = "http://localhost:8080/";
    try {
        // Send a POST request to the load balancer to solve the matrix
        const response = await fetch(`${loadBalancerUrl}/solve`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: matrixToString(matrix), // Send the matrix as a string
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        // Get the unique SolutionId from the response
        const data = await response.json();
        const resultId = data.id;

        console.log('Solution ID received:', resultId);

        // Now use the SolutionId to retrieve the result from the database
        const result = await getResultFromDatabase(resultId);
        console.log('Solution from the database:', result);
    } catch (error) {
        console.error('Failed to send matrix or retrieve result:', error);
    }
}

async function getResultFromDatabase(resultId) {
    try {
      // Send a GET request to the load balancer to retrieve the result
      const response = await fetch(`${loadBalancerUrl}/result/${resultId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .then(response => response.json())
      .then(data => console.log(data))
      .catch(error => console.error('Error:', error));
  
      if (!response.ok) {
        throw new Error(`Error: ${response.statusText}`);
      }
  
      // Get the result from the database
      const resultData = await response.json();
      return resultData;
    } catch (error) {
      console.error('Failed to retrieve result from the database:', error);
    }
}

function matrixToString(matrix) {
    return matrix
      .map(row => row.join(', ')) // Join each row's elements with a comma and space
      .join('\n'); // Join each row with a newline character
}

function solve(){
    sendMatrixAndGetId(getdMatrixFromHtmL());
}