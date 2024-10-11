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

async function SendRequest() {
  try {
    matrix = getdMatrixFromHtmL();
    userid = null;
    const response = fetch('http://localhost/server/solve', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ matrix, userid })
    })
    .then(response => response.text())
    .then(data => console.log(data))
  }
  catch (error) {
    error => console.error('Error:', error);
  };
}

function solve(){
  SendRequest();
}

async function registerUser(username, password) {
  const response = await fetch('/api/auth/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
  });
  const data = await response.json();
  console.log(data);
}

async function loginUser(username, password) {
  const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
  });
  const data = await response.json();
  localStorage.setItem('token', data.token);  // Save the token for future requests
  console.log('Logged in');
}

async function getSolutionHistory() {
  const token = localStorage.getItem('token');
  const response = await fetch('/api/solutions/history', {
      method: 'GET',
      headers: {
          'Authorization': `Bearer ${token}`
      }
  });
  const data = await response.json();
  console.log('Solution History:', data);
}
