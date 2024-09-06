/// <reference types="cypress" />
/// <reference types="testing-library/dom" />

describe('Home Page', () => {
  beforeEach(()=>{
    cy.visit('home.html')
  })

  describe('Login', () =>{
    it('with incorrect login', ()=>{
      cy.findByLabelText('Username').type('Wrong')
      cy.findByLabelText('Password').type('Data')
      cy.findByRole('button', {name: /Sign In/i}).click()
      cy.findByRole('alert', 'Wrong Username or Password')
      //TODO: The error just repeats and is not deleted when trying again. This might be a bug with the design
    })

    it('is able to login', () => {
      cy.findByLabelText('Username').type('john.doe')
      cy.findByLabelText('Password').type('abc123')
      cy.findByRole('button', {name: /Sign In/i}).click()
      cy.getCookie('ACTS_Session').should('exist')
      //TODO: Login tile does not go away once logged in. Does disappear when refreashing the page
      cy.findByText('Duty History').should('be.visible')
    })
  })
    describe('Duty History', () =>{
      beforeEach(()=>{
        cy.visit('home.html')
        cy.request('POST', '/account/signin', {"username":"john.doe","password":"abc123"})
        cy.resetDatabase()
        cy.reload()
        cy.findByRole('button', {name: /Add Duty/i}).click()
        cy.findByLabelText('Add Duty').should('be.visible')
        //TODO: Find a way to not need the Waits
        cy.wait(500)
      })

      it('Add and Removed duty', ()=>{
        cy.get('#duty-history-list-group li').should('have.length', 3)
        cy.findByLabelText('Duty Title').type("Calling about your car's extended warranty!")
        cy.findByLabelText('Duty Rank').type("Calling about your car's extended warranty")
        cy.findByLabelText('Start Date').type('2050-01-01')
        cy.findByLabelText('End Date').type('9999-02-01')
        cy.findByRole('button', {name: /Save changes/i}).click()
        cy.get('#duty-history-list-group li').should('have.length', 4)
        cy.findByText("Calling about your car's extended warranty").parents('li').find('button').click()
      })

      it('Add Duty form has validation',()=>{
        cy.findByRole('button', {name: /Save changes/i}).click()
        cy.findByText('The start date must come before the end date!').should('be.visible')
        cy.findAllByText('Must have a value.').should('have.length', 4).and('be.visible')
        cy.get('.is-invalid').should('have.length', 4)
        cy.findByLabelText('Duty Title').type('abc').blur().parent().findByText('Must have a value.').should('not.be.visible')
        cy.get('.is-invalid').should('have.length', 3)
        cy.findByLabelText('Duty Rank').type('abc{enter}').parent().findByText('Must have a value.').should('not.be.visible')
        cy.get('.is-invalid').should('have.length', 2)
        cy.findByLabelText('Start Date').type('2024-01-01').blur().parent().findByText('Must have a value.').should('not.be.visible')
        cy.get('.is-invalid').should('have.length', 1)
        cy.findByLabelText('End Date').type('2024-01-01{upArrow}').parent().findByText('Must have a value.').should('not.be.visible')
        cy.get('.is-invalid').should('have.length', 0)
      })

      it('Add Duty form does close', ()=>{
        //Visiblity is already being check, just showing a different way I can check if the modal is displayed
        cy.get('#dutyModal').should('be.visible')
        cy.get('.btn-close').should('be.visible').click()
        cy.get('#dutyModal').should('not.be.visible')
        cy.findByRole('button', {name: /Add Duty/i}).click()
        cy.get('#dutyModal').should('be.visible')
        cy.wait(500)
        cy.get('.btn-secondary').should('be.visible').click()
      })

      it('Start and End date validation', ()=>{
        cy.findByLabelText('Duty Title').type('abcdef').blur()
        cy.findByLabelText('Duty Rank').type('abcdef').blur()
        cy.findByLabelText('Start Date').type('2024-09-01')
        cy.findByLabelText('End Date').type('2024-01-01')
        cy.findByRole('button', {name: /Save changes/i}).click()
        cy.findByText('The start date must come before the end date!').should('be.visible')
        cy.findByLabelText('Start Date').clear().type('2024-09-01')
        cy.findByLabelText('End Date').clear().type('2024-12-01')
        cy.findByRole('button', {name: /Save changes/i}).click()
        cy.findByText('Duty overlaps with an existing duty').should('be.visible')
      })
    })

})