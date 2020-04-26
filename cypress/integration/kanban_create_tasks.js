describe('Kanban Create tests', () => {
    it('Logs in and creates Day, Week and Year goals and cleans them up afterwards', () => {
        cy.visit('https://localhost:5001/')
        cy.contains('Login').click()
        cy.contains('NameProvided').type('cypress')
        cy.contains('Password').type('cypress')
        cy.get('.btn-primary').click()
        cy.contains('Kanban').click()
        cy.wait(500)
        cy.get('#add-year-goal-name').should('be.enabled').type('kek1{enter}')
        cy.get('#add-week-goal-name').should('be.enabled').type('kek2{enter}')
        cy.get('#add-day-goal-name').should('be.enabled').type('kek3{enter}')
        cy.wait(500)
        cy.contains('kek1').siblings(':contains("Delete")').click()
        cy.contains('kek2').siblings(':contains("Delete")').click()
        cy.contains('kek3').siblings(':contains("Delete")').click()
        cy.get(':contains("kek1")').should('not.exist');
        cy.get(':contains("kek2")').should('not.exist');
        cy.get(':contains("kek3")').should('not.exist');
    })
})