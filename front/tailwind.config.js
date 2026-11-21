/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      fontFamily: {
        display: ['"Bebas Neue"', 'sans-serif'],
        body: ['Inter', 'sans-serif'],
      },
      colors: {
        cinema: {
          black: '#0a0a0f',
          dark: '#12121a',
          gray: '#1a1a24',
          accent: '#e50914', // Netflix red-ish
          gold: '#f5c518',   // IMDb gold
        },
      },
    },
  },
  plugins: [],
}