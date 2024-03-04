/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [ "./Components/**/*.razor" ],
    theme: {
        extend: {
            screens: {
                "lg": "960px",
                "xl": "1280px"
            },
            colors: {
                "primary": "#fb8b24",
                "neutral": "#ffffff",
                "second": "#d4cfcc",
                "attention": "#f54949",
                "muted": "#454545",
                "background": "#f4f4f4"
            },
            fontFamily: {
                "sans": "Roboto",
                "mono": "Roboto Mono"
            },
            boxShadow: {
                'top' : '0 -1px 3px 0 rgba(0, 0, 0, 0.1)',
            }
        },
    },
    plugins: [],
}

