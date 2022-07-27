describe('find and edit partner', function() {
  it('logs in with user demo and modifies partner', function() {
    cy.server()
    cy.visit('/Partner/Partners/MaintainPartners')
    cy.get('#txtEmail').should("be.visible")
    cy.get('#txtEmail').type('demo')
    cy.get('#txtPassword').type('demo')
    cy.route('POST','**/Login').as('Login')
    cy.get('#btnLogin').click()
    cy.wait('@Login')
    cy.get('#navbarDropdownUser').click()
    cy.wait(500) // wait for dropdown to drop down
    cy.get('#logout').should("be.visible")

    cy.get('#btnFilter').click()
    cy.get('input[name="AFamilyNameOrOrganisation"]').type('jacob')
    cy.route('POST','**FindPartners').as('FindPartners')
    cy.get('#btnSearch').click()
    cy.wait('@FindPartners')
    cy.get('#partner43012945 #btnEditPartner43012945').should("be.visible").click()
    cy.get('#modal_space input[name="PFamily_p_first_name_c"]')
        .should('have.value', "Holger")
        .click().clear().click().type('Albrecht')
    cy.get('#modal_space [href="#contactdetails"]').click()
    cy.get('#modal_space input[name="p_default_email_address_c"]')
        .should('have.value', "holger.jacobi@roland-juenemann.us")
        .click().clear().click().type('albrecht.jacobi@roland-juenemann.us')
    // switch focus, to get consent modal
    cy.get('#modal_space input[name="p_default_phone_landline_c"]').click()
    // confirm consent
    cy.wait(500)
    cy.get('#modal_space #btnSubmitChangesConsent').click()

    cy.get('#modal_space input[name="p_no_solicitations_l"]')
        .should('not.be.checked')
        .check()

    cy.route('POST','**SavePartner').as('SavePartner')
    cy.get('#modal_space #btnSavePartner').click()
    cy.wait('@SavePartner')
    cy.get('#message').should("be.visible").should("contain", 'Successfully saved')

    cy.visit('/Partner/Partners/MaintainPartners')
    cy.get('#btnFilter').click()
    cy.get('input[name="AFamilyNameOrOrganisation"]').click().type('jacob')
    cy.route('POST','**FindPartners').as('FindPartners')
    cy.get('#btnSearch').click()
    cy.wait('@FindPartners')
    cy.get('#partner43012945 #btnEditPartner43012945').should("be.visible").click()
    cy.get('#modal_space input[name="PFamily_p_first_name_c"]')
        .should('have.value', "Albrecht")
        .click().clear().click().type('Holger')
    cy.get('#modal_space [href="#contactdetails"]').click()
    cy.get('#modal_space input[name="p_default_email_address_c"]')
        .should('have.value', "albrecht.jacobi@roland-juenemann.us")
        .click().clear().click().type('holger.jacobi@roland-juenemann.us')
    // switch focus, to get consent modal
    cy.get('#modal_space input[name="p_default_phone_landline_c"]').click()
    // confirm consent
    cy.wait(500)
    cy.get('#modal_space #btnSubmitChangesConsent').click()
    cy.get('#modal_space input[name="p_no_solicitations_l"]')
        .should('be.checked')
        .uncheck()

    cy.route('POST','**SavePartner').as('SavePartner2')
    cy.get('#modal_space #btnSavePartner').click()
    cy.wait('@SavePartner2')
    cy.get('#message').should("be.visible")
    cy.get('#message').should("contain", 'Successfully saved')

    cy.get('#navbarDropdownUser').click()
    cy.wait(500) // wait for dropdown to drop down
    cy.get('#logout').click();
  })
})
