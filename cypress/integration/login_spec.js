describe('simple login', function() {
  it('logs in with user demo', function() {
    cy.visit('/')
    cy.get('#txtEmail').type('demo')
    cy.get('#txtPassword').type('demo')
    cy.server()
    cy.route("POST", "/api/serverSessionManager.asmx/GetVersion", []).as("login")
    cy.get('#btnLogin').click()
    cy.wait("@login")
    cy.get('#logout').click();
  })
  it('logs in with user DEMO', function() {
    cy.visit('/')
    cy.get('#txtEmail').type('DEMO')
    cy.get('#txtPassword').type('demo')
    cy.get('#btnLogin').click()
    cy.server()
    cy.route("POST", "/api/serverSessionManager.asmx/GetVersion", []).as("login")
    cy.wait("@login")
    cy.get('#logout').click();
  })
  it('fails with wrong password', function() {
    cy.visit('/')
    cy.get('#txtEmail').type('DEMO')
    cy.get('#txtPassword').type('demo1234')
    cy.get('#btnLogin').click()
    cy.get('#message').contains('Wrong username or password')
  })
})
