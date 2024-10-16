function setupForm() {
    const numEqs = parseInt(document.getElementById('num-eqs').value);
<<<<<<< HEAD
<<<<<<< HEAD
=======
    const numVars = parseInt(document.getElementById('num-vars').value);
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
>>>>>>> 790bda1 (implement authorization system)
    const equationsDiv = document.getElementById('equations');
    equationsDiv.innerHTML = ''; // Clear previous equations

    // Create inputs for each equation
    for (let i = 0; i < numEqs; i++) {
        let equationHtml = `<br></br>`;
<<<<<<< HEAD
<<<<<<< HEAD
        for (let j = 0; j < numEqs; j++) {
            equationHtml += `<input type="text" class="coef" data-eq="${i}" data-var="${j}" placeholder="(${i + 1}; ${j + 1})">`;
            if (j < numEqs - 1) {
=======
        for (let j = 0; j < numVars; j++) {
            equationHtml += `<input type="text" class="coef" data-eq="${i}" data-var="${j}" placeholder="(${i + 1}; ${j + 1})">`;
            if (j < numVars - 1) {
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
        for (let j = 0; j < numEqs; j++) {
            equationHtml += `<input type="text" class="coef" data-eq="${i}" data-var="${j}" placeholder="(${i + 1}; ${j + 1})">`;
            if (j < numEqs - 1) {
>>>>>>> 790bda1 (implement authorization system)
                equationHtml += ' + ';
            }
        }
        equationHtml += ` = <input type="text" class="const" data-eq="${i}" placeholder="b${i + 1}">`;
        equationsDiv.innerHTML += equationHtml;
    }

    // Show the solver form
    document.getElementById('setup-form').style.display = 'none';
    document.getElementById('solver-form').style.display = 'block';
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 790bda1 (implement authorization system)

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
<<<<<<< HEAD
=======
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
>>>>>>> 790bda1 (implement authorization system)
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

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 5821ea7 (Add data visualization, create histoty page)
function onPageLoad(){
  const path = window.location.pathname;
  if (path.endsWith('index.html')){
    const button = document.getElementById("login-button");
    if (!localStorage.getItem('token')) {
      button.innerText = "Log In/Register";
      button.onclick = function() {
        window.location.href = 'login.html';
      };
    }
    else {
      const header = document.querySelector(".header");
      const historyButton = document.createElement("button");
      historyButton.id = "history-button";
      historyButton.innerText = "Browse History";
      historyButton.onclick = function () {
        window.location.href = 'history.html';
      };
      header.insertBefore(historyButton, button);
      button.innerText = "Log Out";
      button.onclick = LogOut;
    }
  }
  else if (path.endsWith('history.html')){
    getSolutionHistory();
  }
}
window.onload = onPageLoad;

function LogOut(){
  localStorage.removeItem('token');
  const button = document.getElementById("login-button");
  button.innerText = "Log In/Register";
  button.onclick = function() {
    window.location.href = 'login.html';
  };
  window.location.href = 'index.html';
  showNotification("Logged Out");
}

async function SolveRequest() {
  matrix = getdMatrixFromHtmL();
  if (IsValidMatrix(matrix)) {
    try {
      usertoken = localStorage.getItem('token');
      const response = await fetch('http://localhost/server/solve', {
        method: 'POST',
        headers: {
          // 'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ matrix, usertoken })
      });
      
      if (response.ok) {
        if (response.headers.get('Content-Type').includes('application/json')){
          const responseData = await response.json();
          console.log(responseData);
  
          const solution = responseData.solution;
          const solutionDiv = document.getElementById("Solution");
          solutionDiv.innerHTML = `<p>Solution: ${solution.join(", ")}</p>`;
          showNotification("Successfully solved!");
        }
        else {
          showNotification(await response.text());
        }
      }
<<<<<<< HEAD
    }
    catch (error) {
      error => console.error('Error:', error);
    };
  }
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
=======
async function SendRequest() {
=======
async function SolveRequest() {
>>>>>>> 790bda1 (implement authorization system)
  try {
    matrix = getdMatrixFromHtmL();
    usertoken = localStorage.getItem('token');
    const response = await fetch('http://localhost/server/solve', {
      method: 'POST',
      headers: {
        // 'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ matrix, usertoken })
    });

    if (response.ok) {
      const responseData = await response.json();
      console.log(responseData);

      const solution = responseData.solution;
      const solutionDiv = document.getElementById("Solution");
      solutionDiv.innerHTML = `<p>Solution: ${solution.join(", ")}</p>`;
      showNotification("Successfully solved!");
=======
>>>>>>> 387a279 (Add error messages and data validation)
    }
    catch (error) {
      error => console.error('Error:', error);
    };
  }
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

<<<<<<< HEAD
async function registerUser(username, password) {
  const response = await fetch('/api/auth/register', {
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
async function loginUser() {
  const username = document.getElementById("login-username").value;
  const password = document.getElementById("login-password").value;
  const response = await fetch('http://localhost/server/login', {
>>>>>>> 790bda1 (implement authorization system)
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
  });
<<<<<<< HEAD
<<<<<<< HEAD
  if (response.status != 401) {
    const data = await response.text();
    localStorage.setItem('token', data);  // Save the token for future requests
    showNotification('Logged in');
  } 
  else {
    showNotification("Error while logging in...");
  }
  // window.location.href = 'index.html';
=======
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
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
  if (response.status != 401) {
    const data = await response.text();
    localStorage.setItem('token', data);  // Save the token for future requests
    showNotification('Logged in');
  } 
  else {
    showNotification("Error while logging in...");
  }
  // window.location.href = 'index.html';
>>>>>>> 790bda1 (implement authorization system)
}

async function getSolutionHistory() {
  const token = localStorage.getItem('token');
<<<<<<< HEAD
<<<<<<< HEAD
  const response = await fetch('http://localhost/server/history', {
=======
  const response = await fetch('/api/solutions/history', {
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
  const response = await fetch('http://localhost/server/history', {
>>>>>>> 790bda1 (implement authorization system)
      method: 'GET',
      headers: {
          'Authorization': `Bearer ${token}`
      }
  });
  const data = await response.json();
  console.log('Solution History:', data);
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 5821ea7 (Add data visualization, create histoty page)
  
  const historyTableBody = document.getElementById('solution-history');
  historyTableBody.innerHTML = '';  // Clear the table body

  data.forEach((solution, index) => {
      const row = document.createElement('tr');

      // Solution ID
      const solutionIdCell = document.createElement('td');
      solutionIdCell.innerText = index + 1;
      row.appendChild(solutionIdCell);

      // Solution Matrix
      const matrixCell = document.createElement('td');
      matrixCell.innerText = solution.solutionmatrix;
      row.appendChild(matrixCell);

      // Solution Result
      const resultCell = document.createElement('td');
      resultCell.innerText = solution.solutionresult;
      row.appendChild(resultCell);

      // Action - Delete Button
      const actionCell = document.createElement('td');
      const deleteButton = document.createElement('button');
      deleteButton.classList.add('delete-btn');
      deleteButton.innerText = 'Delete';
      deleteButton.onclick = function() {
        DeleteSolutionFromHistory(solution.solutionid);
      };
      actionCell.appendChild(deleteButton);
      row.appendChild(actionCell);

      // Append the row to the table
      historyTableBody.appendChild(row);
  });
}

<<<<<<< HEAD
<<<<<<< HEAD
async function DeleteSolutionFromHistory(solutionid) {
=======
async function DeleteSolutionFromHistory(relativeid) {
>>>>>>> 29a9675 (Implement History System)
=======
async function DeleteSolutionFromHistory(solutionid) {
>>>>>>> 5821ea7 (Add data visualization, create histoty page)
  const token = localStorage.getItem('token');
  const response = await fetch('http://localhost/server/delete', {
    method: 'DELETE',
    headers: { 
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
   },
<<<<<<< HEAD
<<<<<<< HEAD
    body: solutionid
  });
  const data = await response.text();
  console.log(data);
  getSolutionHistory();
}

function showNotification(message) {
  const notificationContainer = document.getElementById('notification-container');
  
  const notification = document.createElement('div');
  notification.className = 'notification';
  notification.textContent = message;
  
  notificationContainer.appendChild(notification);
  
  setTimeout(() => {
      notification.classList.add('fade-out');
      setTimeout(() => {
          notificationContainer.removeChild(notification);
      }, 500); 
  }, 2000);
}

function IsValidMatrix(matrix){
  matrix.forEach(array => {
    array.forEach(data => {
      if (isNaN(data)){
        showNotification("All cells must be filled!");
        return false;
      }
    })
  });
  return true;
<<<<<<< HEAD
}
=======
}
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
    body: relativeid
  });
  const data = await response.text();
  console.log(data);
}
>>>>>>> 29a9675 (Implement History System)
=======
    body: solutionid
  });
  const data = await response.text();
  console.log(data);
  getSolutionHistory();
}

function showNotification(message) {
  const notificationContainer = document.getElementById('notification-container');
  
  const notification = document.createElement('div');
  notification.className = 'notification';
  notification.textContent = message;
  
  notificationContainer.appendChild(notification);
  
  setTimeout(() => {
      notification.classList.add('fade-out');
      setTimeout(() => {
          notificationContainer.removeChild(notification);
      }, 500); 
  }, 2000);
}
>>>>>>> 5821ea7 (Add data visualization, create histoty page)
=======
}
>>>>>>> 387a279 (Add error messages and data validation)
