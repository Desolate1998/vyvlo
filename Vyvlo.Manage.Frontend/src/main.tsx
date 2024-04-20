import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import { FluentProvider, teamsLightTheme } from '@fluentui/react-components'
import { darkTheme, lightTheme } from './Types/ApplicationTheme.tsx'
import './index.css'
import { ThemeProvider } from './Contexts/ThemeContext.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <ThemeProvider>
    <App />
  </ThemeProvider>
)
