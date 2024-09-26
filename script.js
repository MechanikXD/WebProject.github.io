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
/*
function solve() {
    const numEqs = parseInt(document.getElementById('num-eqs').value);
    const numVars = parseInt(document.getElementById('num-vars').value);
    const coefficients = [];
    const constants = [];

    // Gather coefficients and constants from the form
    for (let i = 0; i < numEqs; i++) {
        const rowCoeffs = [];
        for (let j = 0; j < numVars; j++) {
            rowCoeffs.push(parseFloat(document.querySelector(`input[data-eq="${i}"][data-var="${j}"]`).value) || 0);
        }
        coefficients.push(rowCoeffs);
        constants.push(parseFloat(document.querySelector(`input[data-eq="${i}"].const`).value) || 0);
    }

    // Solve the system using a basic method for demonstration
    // This method assumes that the number of equations equals the number of variables
    if (numEqs !== numVars) {
        document.getElementById('solution').textContent = 'The number of equations must equal the number of variables.';
        return;
    }

    const result = gaussJordan(coefficients, constants);
    document.getElementById('solution').textContent = result ? result.join(', ') : 'No unique solution or no solution.';
}
*/

/*
function gaussJordan(a, b) {
    const n = a.length;
    const x = new Array(n).fill(0);
    const aug = a.map((row, i) => row.concat(b[i]));

    for (let i = 0; i < n; i++) {
        let maxRow = i;
        for (let k = i + 1; k < n; k++) {
            if (Math.abs(aug[k][i]) > Math.abs(aug[maxRow][i])) {
                maxRow = k;
            }
        }
        if (aug[maxRow][i] === 0) return null; // No unique solution

        // Swap rows
        [aug[i], aug[maxRow]] = [aug[maxRow], aug[i]];

        // Make leading coefficient 1 and eliminate column
        for (let j = i + 1; j < n; j++) {
            const ratio = aug[j][i] / aug[i][i];
            for (let k = i; k < n + 1; k++) {
                aug[j][k] -= ratio * aug[i][k];
            }
        }
    }

    // Back substitution
    for (let i = n - 1; i >= 0; i--) {
        x[i] = aug[i][n] / aug[i][i];
        for (let j = 0; j < i; j++) {
            aug[j][n] -= aug[j][i] * x[i];
        }
    }
    return x;
}
*/