function setupForm() {
    const numEqs = parseInt(document.getElementById('num-eqs').value);
    const equationsDiv = document.getElementById('equations');
    equationsDiv.innerHTML = ''; // Clear previous equations

    // Create inputs for each equation
    for (let i = 0; i < numEqs; i++) {
        let equationHtml = `<br></br>`;
        for (let j = 0; j < numEqs; j++) {
            equationHtml += `<input type="text" class="coef" data-eq="${i}" data-var="${j}" placeholder="(${i + 1}; ${j + 1})">`;
            if (j < numEqs - 1) {
                equationHtml += ' + ';
            }
        }
        equationHtml += ` = <input type="text" class="const" data-eq="${i}" placeholder="b${i + 1}">`;
        equationsDiv.innerHTML += equationHtml;
    }

    // Show the solver form
    document.getElementById('setup-form').style.display = 'none';
    document.getElementById('solver-form').style.display = 'block';

    // Handle arrow key navigation
    document.addEventListener('keydown', function (event) {
      const currentElement = document.activeElement;
      if (currentElement.tagName.toLowerCase() === 'input') {
        const coefElements = Array.from(document.querySelectorAll('.coef, .const'));
  
        // Get the index of the current active element
        const currentIndex = coefElements.indexOf(currentElement);
  
        // Define movement based on arrow keys
        switch (event.key) {
          case 'ArrowRight':
            if (currentIndex < coefElements.length - 1) {
              coefElements[currentIndex + 1].focus(); // Move to the right
            }
            event.preventDefault();
            break;
          case 'ArrowLeft':
            if (currentIndex > 0) {
              coefElements[currentIndex - 1].focus(); // Move to the left
            }
            event.preventDefault();
            break;
          case 'ArrowDown':
            if (currentElement.classList.contains('coef')) {
              // Move to the same column but next row
              const eq = parseInt(currentElement.getAttribute('data-eq'));
              const variable = currentElement.getAttribute('data-var');
              const nextElement = document.querySelector(`input[data-eq="${eq + 1}"][data-var="${variable}"]`);
              if (nextElement) {
                nextElement.focus(); // Move down
              }
            } else if (currentElement.classList.contains('const')) {
              // Move down to the next constant
              const eq = parseInt(currentElement.getAttribute('data-eq'));
              const nextElement = document.querySelector(`input.const[data-eq="${eq + 1}"]`);
              if (nextElement) {
                nextElement.focus(); // Move down
              }
            }
            event.preventDefault();
            break;
          case 'ArrowUp':
            if (currentElement.classList.contains('coef')) {
              // Move to the same column but previous row
              const eq = parseInt(currentElement.getAttribute('data-eq'));
              const variable = currentElement.getAttribute('data-var');
              const prevElement = document.querySelector(`input[data-eq="${eq - 1}"][data-var="${variable}"]`);
              if (prevElement) {
                prevElement.focus(); // Move up
              }
            } else if (currentElement.classList.contains('const')) {
              // Move up to the previous constant
              const eq = parseInt(currentElement.getAttribute('data-eq'));
              const prevElement = document.querySelector(`input.const[data-eq="${eq - 1}"]`);
              if (prevElement) {
                prevElement.focus(); // Move up
              }
            }
            event.preventDefault();
            break;
        }
      }
    }
  )
}

function focusNextInput(row, col) {
  let nextInput = document.querySelector(`input[data-eq="${row}"][data-var="${col}"]`);
  if (nextInput) {
      nextInput.focus();
  }
}
// Button to return to the main page.
function goToMain() {
  window.location.href = 'index.html';
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

async function SolveRequest() {
  try {
    matrix = getdMatrixFromHtmL();
    usertoken = localStorage.getItem('token');
    const response = fetch('http://localhost/server/solve', {
      method: 'POST',
      headers: {
        // 'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ matrix, usertoken })
    })
    .then(response => response.json())
    .then(data => console.log(data))
  }
  catch (error) {
    error => console.error('Error:', error);
  };
}

async function registerUser() {
  const username = document.getElementById("register-username").value;
  const password = document.getElementById("register-password").value;
  try {
    if (password == document.getElementById("register-password-confirm").value){
      const response = fetch('http://localhost/server/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
      })
      .then(response => response.text())
      .then(data => console.log(data));
    }
    else {
      console.log("Password doesn't match");
    }
  }
  catch (error){
    error => console.error('Error:', error);
  }
}

async function loginUser() {
  const username = document.getElementById("login-username").value;
  const password = document.getElementById("login-password").value;
  const response = await fetch('http://localhost/server/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
  });
  if (response.status != 401) {
    const data = await response.text();
    localStorage.setItem('token', data);  // Save the token for future requests
    console.log('Logged in');
  } 
  else {
    console.error("Response is not OK");
  }
  // window.location.href = 'index.html';
}

async function getSolutionHistory() {
  const token = localStorage.getItem('token');
  const response = await fetch('http://localhost/server/history', {
      method: 'GET',
      headers: {
          'Authorization': `Bearer ${token}`
      }
  });
  const data = await response.json();
  console.log('Solution History:', data);
}

async function DeleteSolutionFromHistory(relativeid) {
  const token = localStorage.getItem('token');
  const response = await fetch('http://localhost/server/delete', {
    method: 'DELETE',
    headers: { 
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
   },
    body: relativeid
  });
  const data = await response.text();
  console.log(data);
}