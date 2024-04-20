import React, { useEffect, useState } from 'react';
import { useAuth } from '../../Contexts/AuthContext';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoginPage from '../../Pages/Login/Login';
import { useStore } from '../../Contexts/StoreContext';
import { FirstStoreCreationPage } from '../../Pages/FirstStoreCreation/FirstStoreCreation';
import { Card, FluentProvider, ProgressBar, teamsDarkTheme, webDarkTheme, webLightTheme } from '@fluentui/react-components';
import { lightTheme } from '../../Types/ApplicationTheme';
import { useTheme } from '../../Contexts/ThemeContext';
import { Dashboard } from '../../Pages/Dashboard/Dashboard';
import { Header } from '../Header/Header';
import { Sidebar } from '../Sidebar/Sidebar';
import { ManageStore } from '../../Pages/ManageStore/ManageStore';
import { request } from '../../Types/Agnet';
import { ErrorOr } from '../../Types/ErrorOr';
import { User } from '../../Types/User';
import { ManageCategories } from '../../Pages/ManageCategories/ManageCategories';

export const AppRouter = () => {
    const { login ,isAuthenticated} = useAuth();
    const { setStoresData, setCurrentStoreId, currentStoreId } = useStore()
    const { theme} = useTheme();
    const [isMobile, setIsMobile] = useState(window.innerWidth <= 768); // Adjust breakpoint as needed
    const [sidebarVisible, setSidebarVisible] = useState(true);
    const [loading, setLoading] = useState(false)

    const tryAutoLogin = async () => {
        const user = localStorage.getItem('user');
        if (user !== null) {
            try {
                setLoading(true)
                let response = await request.post<ErrorOr<User>>('/api/Authentication/RefreshLogin', { RefreshToken: (JSON.parse(user) as User).refreshToken });
                if (response.isError) {
                    localStorage.clear();
                } else {
                    setCurrentStoreId(Object.keys(response.value.userStores)[0])
                    setStoresData(response.value.userStores)
                    login(response.value)
                }
            } catch (error) {
                localStorage.clear();
            }finally{
                setLoading(false)
            }
        }
    }

    useEffect(() => {
        if (!isAuthenticated) {
            tryAutoLogin();
        }
    }, []); 

    useEffect(() => {
        const handleResize = () => {
            setIsMobile(window.innerWidth <= 768); // Adjust breakpoint as needed
        };
      
        window.addEventListener('resize', handleResize);

        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, []);

    const toggleSidebar = () => {
        setSidebarVisible(!sidebarVisible);
    };

    if(loading){
        return <ProgressBar/>
    }

    if (!isAuthenticated) {
        return (
            <Router>
                <Routes>
                    <Route path="*" Component={LoginPage} />
                </Routes>
            </Router>
        );
    } else if (currentStoreId == null) {
        return (
            <FluentProvider theme={lightTheme}>
                <FirstStoreCreationPage />
            </FluentProvider>
        );
    }


    return (
        <FluentProvider theme={theme}>
            <Router>
                <div style={{ display: 'grid', gridTemplateRows: '10% auto', gridTemplateColumns: isMobile ? '100%' : `${sidebarVisible ? '15%' : '0'} auto`, height: '100vh' }}>
                    <div style={{ gridColumn: '1 / span 2' }}><Header toggle={toggleSidebar} /></div>
                    {isMobile && sidebarVisible && <Card
                        style={{ zIndex: 1000, gridColumn: '1', gridRow: '2 / 3', overflowY: 'auto' }}
                    ><Sidebar /></Card>}
                    {sidebarVisible && !isMobile && (
                        <Card style={{ gridColumn: '1', gridRow: '2 / 3', overflowY: 'auto' }}><Sidebar /></Card>
                    )}
                    <div style={{ padding: '10px', gridColumn: isMobile ? '1 / span 2' : '2', gridRow: '2 / 3', overflowY: 'auto' }}>
                        <Routes>
                            <Route path="*" Component={Dashboard} />
                            <Route path="manage-store" Component={ManageStore} />
                            <Route path="manage-categories" Component={ManageCategories} />
                        </Routes>
                    </div>
                </div>
            </Router>
        </FluentProvider>
    );
};
