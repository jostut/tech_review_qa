describe('Home Page', () => {
  afterEach(()=>{
    cy.request('/account/signout')
  })
  describe('Login', () =>{
    it('is able to login', () => {
      cy.visit('home.html')
    })
  })
})