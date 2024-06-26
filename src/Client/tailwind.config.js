/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [ "./Components/**/*.razor" ],
    theme: {
        container: {
            center: true
        },
        screens: {
            "sm": "480px",
            "md": "768px",
            "lg": "960px",
            "xl": "1280px"
        },
        extend: {
            colors: {
                "primary": "#fb8b24",
                "dark-primary": "#f98212",
                "neutral": "#ffffff",
                "dark-neutral": "#fafafa",
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
    future: {
        hoverOnlyWhenSupported: true,
    },
    plugins: [],
}

