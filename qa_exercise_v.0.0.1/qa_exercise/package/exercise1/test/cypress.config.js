const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl: 'http://localhost:5204/',
    setupNodeEvents(on, config) {
    },
  },
});
