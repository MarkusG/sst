/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./src/**/*.{js,jsx,ts,tsx}",
    ],
    theme: {
        colors: {
            white: '#fff',
            black: '#000',
            gray: {
                50: '#f8f8f8',
                100: '#eeeeee',
                150: '#e5e5e5',
                200: '#dedede',
                300: '#bebebe',
                400: '#9e9e9e',
                500: '#717171',
                600: '#555555',
                700: '#3a3a3a',
                800: '#222222',
                900: '#030303'
            },
            primary: {
                100: '#d1f4ff',
                200: '#b4e5ff',
                300: '#6fc8ff',
                400: '#00a9ff',
                500: '#0079d8',
                600: '#005aa8',
                700: '#003d7f',
                800: '#002258',
                900: '#00002d',
                1000: '#010015'
            },
            accent: {
                100: '#f3e8ff',
                200: '#e5d4ff',
                300: '#caacff',
                400: '#b180fc',
                500: '#864ad2',
                600: '#6635a3',
                700: '#491c7b',
                800: '#2d0255',
                900: '#0d002a',
                1000: '#030013'
            },
            green: {
                100: '#ddf7d8',
                200: '#c6eabe',
                300: '#94d186',
                400: '#59b842',
                500: '#0b8b00',
                600: '#006900',
                700: '#004b00',
                800: '#002e00',
                900: '#000800',
                1000: '#000100'
            },
            yellow: {
                100: '#f6f0ca',
                200: '#e8e0aa',
                300: '#d0bf5e',
                400: '#b99e00',
                500: '#8e6f00',
                600: '#6d5300',
                700: '#4e3800',
                800: '#311e00',
                900: '#0d0000',
                1000: '#020000'
            },
            red: {
                100: '#ffe3db',
                200: '#ffcdc3',
                300: '#ffa08f',
                400: '#fa6a57',
                500: '#cc2a1b',
                600: '#9e170c',
                700: '#760000',
                800: '#4f0000',
                900: '#200000',
                1000: '#0b0000'
            },
        },
        extend: {},
    },
    plugins: [],
}
