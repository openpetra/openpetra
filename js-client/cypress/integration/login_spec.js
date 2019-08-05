describe('simple login', function() {
  it('logs in with user demo', function() {
    cy.visit('/')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('demo')
    cy.get('#txtPassword').type('demo')
    cy.get('#btnLogin').click()
    cy.get('#logout').should("be.visible")
    cy.get('#logout').click();
  })
  it('logs in with user DEMO', function() {
    cy.visit('/')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('DEMO')
    cy.get('#txtPassword').type('demo')
    cy.get('#btnLogin').click()
    cy.get('#logout').should("be.visible")
    cy.get('#logout').click();
  })
  it('fails with wrong password', function() {
    cy.visit('/')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('DEMO')
    cy.get('#txtPassword').type('demo1234')
    cy.get('#btnLogin').click()
    cy.get('#message').should("be.visible")
    cy.get('#message').contains('Wrong username or password')
  })
})
