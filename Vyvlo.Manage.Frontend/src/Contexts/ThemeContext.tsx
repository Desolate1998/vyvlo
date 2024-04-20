import React, { createContext, useContext, useState, ReactNode } from "react";
import { darkTheme, lightTheme } from "../Types/ApplicationTheme";
import { Theme } from "@fluentui/react-components";
import Cookies from 'js-cookie';

interface ThemeContextProps {
    swapTheme: () => void;
    theme:Theme;
    currentTheme: 'light' | 'dark'; 
}

const ThemeContext = createContext<ThemeContextProps | undefined>(undefined);

interface ThemeProviderProps {
    children: ReactNode;
}

export const ThemeProvider: React.FC<ThemeProviderProps> = ({ children }) => {
    const initialTheme = Cookies.get('theme') || 'light';
    const [themeStyle, setThemeStyle] = useState<'light'|'dark'>(initialTheme as 'light' | 'dark');
    const [theme, setTheme] = useState(initialTheme === 'light' ? lightTheme : darkTheme);
    
    const dark = '#212121'
    const light = '#dee2e6'

    var r = document.querySelector(':root');
    if (themeStyle === 'dark') {
        //@ts-ignore
        r!.style.setProperty('--bodybackground', dark);
    } else {
        //@ts-ignore
        r!.style.setProperty('--bodybackground', light);
    }
    
    const swapTheme = () => {
        if (themeStyle === 'light') {
            //@ts-ignore
            r!.style.setProperty('--bodybackground', dark);
            setThemeStyle('dark')
            setTheme(darkTheme)
            Cookies.set('theme', 'dark');
        } else {
            //@ts-ignore
            r!.style.setProperty('--bodybackground', light);
            setThemeStyle('light')
            setTheme(lightTheme)
            Cookies.set('theme', 'light');
        }
    }

    return (
        <ThemeContext.Provider
            value={{
                swapTheme: swapTheme,
                theme:theme,
                currentTheme:themeStyle
            }}
        >
            {children}
        </ThemeContext.Provider>
    );
};

export const useTheme = (): ThemeContextProps => {
    const context = useContext(ThemeContext);
    if (!context) {
        throw new Error("useTheme must be used within a Auth provider");
    }
    return context;
};
