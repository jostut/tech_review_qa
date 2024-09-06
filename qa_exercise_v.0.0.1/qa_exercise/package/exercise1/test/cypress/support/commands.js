import '@testing-library/cypress/add-commands'
// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })

Cypress.Commands.add('resetDatabase', () => {
    cy.request('GET','/astronautduty').then((response)=>{
        const { astronautDuties } = response.body;
        //Yes this is assuming I don't delete the original 3 datasets in a test. If I deside to do that then I will change this
        if(astronautDuties.length != 3){
            for(const astronautDuty of astronautDuties){
                if( ![1, 2, 3].includes(astronautDuty.id)){
                    cy.request('DELETE','/astronautduty', {"id": astronautDuty.id})
                }
            }
        }
    })
})

