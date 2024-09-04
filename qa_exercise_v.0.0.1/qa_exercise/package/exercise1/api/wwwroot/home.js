const authButton = document.querySelector("#auth");
const signinCard = document.querySelector("#sign-in-card");
const dutyHistoryCard = document.querySelector("#duty-history-card");
const dutyHistoryListGroup = document.querySelector('#duty-history-list-group');
const newDutyModal = new bootstrap.Modal('#dutyModal');
const newDutyTitle = document.getElementById("newDutyTitle");
const newDutyRank = document.getElementById("newDutyRank");
const newDutyStart = document.getElementById("newDutyStart");
const newDutyEnd = document.getElementById("newDutyEnd");
const newDutyValidationAlert = document.getElementById("newDutyValidationAlert");
const signInValidationAlert = document.getElementById("signInValidationAlert");
const signedInAsSpan = document.getElementById("signed-in-as");

document.addEventListener("DOMContentLoaded", function () {

    if (checkCookie("ACTS_Session")) {
        hideSignIn();
        getAccountInformation();
        getDutyHistory();
        setTimeout(() => {
            showAccountInfo();
        }, 1000);

    } else {
        hideAccountInfo();
        showSignIn();
    }
});

function showSignIn() {
    if (signinCard.classList.contains("slide-out")) {
        signinCard.classList.remove("slide-out");
    }

    if (signinCard.classList.contains("d-none")) {
        signinCard.classList.remove("d-none");
        signinCard.classList.add("slide-in");
    } else {
        signinCard.classList.remove("slide-out");
        signinCard.classList.add("slide-in");
    }

    authButton.classList.add("d-none");
}

function hideSignIn() {

    signinCard.classList.remove("slide-in");
    signinCard.classList.add("slide-out");
    authButton.classList.remove("d-none");
}

function showAccountInfo() {
    if (dutyHistoryCard.classList.contains("d-none")) {
        dutyHistoryCard.classList.remove("d-none");
        dutyHistoryCard.classList.add("slide-in-from-bottom");
    }

}

function hideAccountInfo() {
    if (!dutyHistoryCard.classList.contains("d-none")) {
        dutyHistoryCard.classList.add("d-none");
    }
}

function hideNewDutyModal() {
    newDutyModal.hide();
}

function showNewDutyModal() {
    newDutyModal.show();
}

function checkCookie(name) {
    // Get all cookies as a single string
    let cookies = document.cookie.split(";");

    // Loop through the cookies
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim();

        // Check if the cookie name matches
        if (cookie.startsWith(name + "=")) {
            return true;
        }
    }

    // Return false if the cookie is not found
    return false;
}

function getCookie(name) {
    let cookieArr = document.cookie.split(";");

    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");

        // Removing whitespace at the beginning of the cookie name and compare it with the given string
        if (name == cookiePair[0].trim()) {
            // Decode the cookie value and return
            return decodeURIComponent(cookiePair[1]);
        }
    }

    // Return null if not found
    return null;
}

function convertBase64ToJsonObject(base64String) {
    let jsonString = atob(base64String);

    // Step 2: Parse the JSON string into an object
    let jsonObject = JSON.parse(jsonString);

    return jsonObject;
}

function formatDate(valdate) {
    if (valdate === null)
        return null;

    let date = new Date(valdate);
    let day = date.getDate().toString().padStart(2, '0');
    let monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    let month = monthNames[date.getMonth()];
    let year = date.getFullYear();

    return `${day}-${month}-${year}`;
}

async function getDutyHistory() {


    const dutyUrl = new URL("https://localhost:7204/astronautduty");


    const res2 = await fetch(dutyUrl);

    if (!res2.ok) {
        throw new Error(`Response status: ${res2.status}`);
    }

    const res2Json = await res2.json();

    res2Json.astronautDuties.forEach(item => {

        const listItem = document.createElement('li');
        listItem.className = 'list-group-item d-flex justify-content-between align-items-center';
        listItem.innerHTML = `<div>
                       <h5 class="mb-1">${item.dutyTitle}</h5>
                       <p class="mb-1">Rank: <span class="badge bg-primary">${item.rank}</span></p>
                       <small>Start Date: ${formatDate(item.dutyStartDate)}</small><br>
                       <small>End Date: ${formatDate(item.dutyEndDate) || ''}</small>
                   </div>
                    <button data-info="${item.id}" class="btn btn-danger btn-sm duty-delete">Delete</button>
                   `;
        dutyHistoryListGroup.appendChild(listItem);

    });
}

async function getAccountInformation() {

    const url = new URL("https://localhost:7204/person");


    const res1 = await fetch(url);

    if (!res1.ok) {
        throw new Error(`Response status: ${res1.status}`);
    }

    const res1Json = await res1.json();

    signedInAsSpan.innerHTML = `User: ${res1Json.person.name}`;
}

document.getElementById("sign-in-form").addEventListener("submit", async (event) => {
    try {
        event.preventDefault();

        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;

        const data = {
            username: username,
            password: password
        };

        const options = {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        };


        const response = await fetch("/account/signin", options);

        if (!response.ok) {
            const responseJson = await response.json();
            let i = 0;
            if (response.status === 401)
                appendsignInValidationAlert(responseJson.message, 'danger');
        } else {
            getAccountInformation();
            getDutyHistory();
            hideSignIn();

            setTimeout(() => {
                showAccountInfo();
            }, 1000);
        }
    } catch (e) {
        console.log(e);
    }


});



function checkTextForValue(val) {
    if (val.trim() !== "") {
        return true;
    } else {
        return false;
    }
}

function checkIfStartDateBeforeEndDate(startDate, endDate) {
    return startDate < endDate;
}



const appendnewDutyValidationAlert = (message, type) => {
    const wrapper = document.createElement('div')
    wrapper.innerHTML = [
        `<div class="alert alert-${type} alert-dismissible" role="alert">`,
        `   <div>${message}</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('')

    newDutyValidationAlert.append(wrapper)
}

const appendsignInValidationAlert = (message, type) => {
    const wrapper = document.createElement('div')
    wrapper.innerHTML = [
        `<div class="alert alert-${type} alert-dismissible" role="alert">`,
        `   <div>${message}</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('')

    signInValidationAlert.append(wrapper)
}

newDutyTitle.addEventListener("change", function () {
    if (checkTextForValue(newDutyTitle.value)) {
        if (newDutyTitle.classList.contains("is-invalid")) {
            newDutyTitle.classList.remove("is-invalid");
        }
        newDutyTitle.classList.add("is-valid");
    }
});

newDutyRank.addEventListener("change", function () {
    if (checkTextForValue(newDutyRank.value)) {
        if (newDutyRank.classList.contains("is-invalid")) {
            newDutyRank.classList.remove("is-invalid");
        }
        newDutyRank.classList.add("is-valid");
    }
});

newDutyStart.addEventListener("change", function () {
    if (checkTextForValue(newDutyStart.value)) {
        if (newDutyStart.classList.contains("is-invalid")) {
            newDutyStart.classList.remove("is-invalid");
        }
        newDutyStart.classList.add("is-valid");
    }
});

newDutyEnd.addEventListener("change", function () {
    if (checkTextForValue(newDutyEnd.value)) {
        if (newDutyEnd.classList.contains("is-invalid")) {
            newDutyEnd.classList.remove("is-invalid");
        }
        newDutyEnd.classList.add("is-valid");
    }
});

document.getElementById("dutyForm").addEventListener("submit", async (event) => {
    try {

        event.preventDefault();
        event.stopPropagation();

        let isValid = true;

        if (!checkTextForValue(newDutyTitle.value)) {
            newDutyTitle.classList.add("is-invalid");
            isValid = false;
        }

        if (!checkTextForValue(newDutyRank.value)) {
            newDutyRank.classList.add("is-invalid");
            isValid = false;
        }

        if (!checkTextForValue(newDutyStart.value)) {
            newDutyStart.classList.add("is-invalid");
            isValid = false;
        }

        if (!checkTextForValue(newDutyEnd.value)) {
            newDutyEnd.classList.add("is-invalid");
            isValid = false;
        }

        if (!checkIfStartDateBeforeEndDate(newDutyStart.value, newDutyEnd.value)) {
            appendnewDutyValidationAlert('The start date must come before the end date!', 'danger');
            isValid = false;
        }

        if (isValid) {

            const data = {
                Rank: newDutyRank.value,
                DutyTitle: newDutyTitle.value,
                DutyStartDate: newDutyStart.value,
                DutyEndDate: newDutyEnd.value
            };

            const options = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            };


            const response = await fetch("/astronautduty", options);

            if (!response.ok) {
                const responseJson = await response.json();
                let i = 0;
                if (response.status === 400)
                    appendnewDutyValidationAlert(responseJson.message, 'danger');
            } else {
                hideNewDutyModal();
                newDutyRank.value = '';
                newDutyTitle.value = '';
                newDutyStart.value = '';
                newDutyEnd.value = '';

                if (newDutyTitle.classList.contains("is-valid")) {
                    newDutyTitle.classList.remove("is-valid");
                }

                if (newDutyRank.classList.contains("is-valid")) {
                    newDutyRank.classList.remove("is-valid");
                }

                if (newDutyStart.classList.contains("is-valid")) {
                    newDutyStart.classList.remove("is-valid");
                }

                if (newDutyEnd.classList.contains("is-valid")) {
                    newDutyEnd.classList.remove("is-valid");
                }

                newDutyValidationAlert.innerHTML = ``;
                dutyHistoryListGroup.innerHTML = ``;
                getAccountInformation();
                getDutyHistory();
            }
        }

    } catch (e) {
        console.log(e);
    }
});

document.getElementById('duty-history-list-group').addEventListener('click', function (event) {
    if (event.target && event.target.nodeName === 'BUTTON') {
        const data = event.target.getAttribute('data-info');
        deleteDuty(data);
    }
});

async function deleteDuty(id) {
    const data = {
        id: id
    };

    const options = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    };


    const response = await fetch("/astronautduty", options);

    if (!response.ok) {

    } else {
        dutyHistoryListGroup.innerHTML = ``;

        getDutyHistory();
    }
}

authButton.addEventListener("click", async () => {
    try {
        let IsAuthenticated = checkCookie("ACTS_Session");
        if (IsAuthenticated) {
            const response = await fetch("/account/signout");
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }

            dutyHistoryListGroup.innerHTML = ``;
            signedInAsSpan.innerHTML = ``;
            document.getElementById("username").value = '';
            document.getElementById("password").value = '';

            hideAccountInfo();
            showSignIn();
        }

    } catch (e) {
        console.log(e);
    }
});