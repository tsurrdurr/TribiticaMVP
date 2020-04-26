describe('Log in log out', () => {
    it('Logs in and out', () => {
        cy.visit('https://localhost:5001/')
        cy.contains('Login').click()
        cy.contains('NameProvided').type('cypress')
        cy.contains('Password').type('cypress')
        cy.get('.btn-primary').click()
        cy.contains('Logout').click()
        cy.contains('Login')
    })
})