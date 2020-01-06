describe('simple login', function() {
  it('logs in with user demo', function() {
    cy.server()
    cy.visit('/')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('demo')
    cy.get('#txtPassword').type('demo')
    cy.route('POST','**/Login').as('Login')
    cy.get('#btnLogin').click()
    cy.wait('@Login')
    cy.get('#navbarDropdownUser').click()
    cy.wait(500) // wait for dropdown to drop down
    cy.get('#logout').should("be.visible")
    cy.get('#logout').click();
  })
  it('logs in with user DEMO', function() {
    cy.server()
    cy.visit('/')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('DEMO')
    cy.get('#txtPassword').type('demo')
    cy.route('POST','**/Login').as('Login')
    cy.get('#btnLogin').click()
    cy.wait('@Login')
    cy.get('#navbarDropdownUser').click()
    cy.wait(500) // wait for dropdown to drop down
    cy.get('#logout').should("be.visible")
    cy.get('#logout').click();
  })
  it('fails with wrong password', function() {
    cy.server()
    cy.visit('/')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('DEMO')
    cy.get('#txtPassword').type('demo1234')
    cy.route('POST','**/Login').as('Login')
    cy.get('#btnLogin').click()
    cy.wait('@Login')
    cy.get('#message').should("be.visible")
    cy.get('#message').contains('Wrong username or password')
  })
})
