import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    defaultCommandTimeout: 10000,
    supportFile: false,
    specPattern: "cypress/**/*.js",
  },
  video: false,
})