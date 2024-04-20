import Login from './Pages/Login/Login'
import { ToasterProvider } from './Contexts/ToasterContext'
import { AuthProvider } from './Contexts/AuthContext'
import { StoreProvider } from './Contexts/StoreContext'
import { AppRouter } from './compnents/AppRouter/AppRouter'

function App() {
  return (
    <AuthProvider>
      <ToasterProvider>
        <StoreProvider>
          <AppRouter/>
        </StoreProvider>
      </ToasterProvider>
    </AuthProvider>
  )
}

export default App
