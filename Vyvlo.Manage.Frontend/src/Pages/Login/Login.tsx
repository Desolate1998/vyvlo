import * as React from 'react';
import { Input, Button, Card, Field, FluentProvider, teamsLightTheme, webLightTheme, CardFooter, CardHeader, ProgressBar, Toaster } from '@fluentui/react-components';
import { useTheme } from '../../Contexts/ThemeContext';
import { lightTheme } from '../../Types/ApplicationTheme';
import { Eye12Regular, Eye12Filled } from '@fluentui/react-icons';
import { Register } from './Register/Register';
import { useState } from 'react';
import logo from '../../assets/Icon.png';
import { ValueError } from '../../Types/ValueError';
import { request } from '../../Types/Agnet';
import { useToaster } from '../../Contexts/ToasterContext';
import { ErrorOr } from '../../Types/ErrorOr';
import { User } from '../../Types/User';
import { useAuth } from '../../Contexts/AuthContext';
import { useStore } from '../../Contexts/StoreContext';

const LoginPage: React.FC = () => {
    const { notify, mainToast } = useToaster()
    const { login } = useAuth();
    const { setStoresData ,setCurrentStoreId} = useStore()
    const [email, setEmail] = useState<ValueError<string>>({ error: '', value: '' });
    const [password, setPassword] = useState<ValueError<string>>({ error: '', value: '' });
    const [registerOpen, setRegisterOpen] = useState(false)
    const [passwordVisible, setPasswordVisible] = React.useState(false);
    const [loading, setLoading] = useState(false)

    const handleLogin = async (event: React.FormEvent) => {
        event.preventDefault();

        if (!email.value.trim()) {
            setEmail(prev => ({ ...prev, error: 'This field is required' }));
        }
        if (!password.value.trim()) {
            setPassword(prev => ({ ...prev, error: 'This field is required' }));
        }

        if (email.error || password.error) {
            return;
        }

        setLoading(true);

        try {
            let response = await request.post<ErrorOr<User>>('/api/Authentication/login', { email: email.value, password: password.value });

            if (response.isError) {
                notify('Login Failed', 'error')
            } else {
                notify('Login Successful', 'success')
                setCurrentStoreId(Object.keys(response.value.userStores)[0])
                setStoresData(response.value.userStores)
                login(response.value)
            }
        } catch (error) {
            notify('Internal Server Error', 'error')
        } finally {
            setLoading(false);
        }
    };

    const handleRegisterStateSwap = () => {
        setRegisterOpen(!registerOpen);
    }

    return (
        <FluentProvider theme={lightTheme}>
            <Toaster
                toasterId={mainToast}
                position="top-end"
                pauseOnHover
                pauseOnWindowBlur
                timeout={1000}
            />
            <Register changeOpen={handleRegisterStateSwap} open={registerOpen} />
            <div style={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                height: '100vh',
                background: 'linear-gradient(130deg, rgba(249,249,249,1) 7%, rgba(107,94,182,1) 59%, rgba(130,79,153,1) 91%)',

            }}>
                <br />
                <Card
                    className='FirstStoreCreationPage-card'>

                    <CardHeader header={<h1 >Sign In</h1>}>
                    </CardHeader>
                    <Field label='Email' validationMessage={email.error} >
                        <Input disabled={loading} placeholder='Please Enter Email Address...' value={email.value} onChange={(_, data) => setEmail({ value: data.value, error: '' })} />
                    </Field>

                    <Field label='Password' validationMessage={password.error}>
                        <Input disabled={loading} placeholder='Please Enter Password...' value={password.value} type={passwordVisible ? 'text' : 'password'} onChange={(_, data) => setPassword({ value: data.value, error: '' })} contentAfter={passwordVisible ? <Eye12Regular style={{ cursor: 'pointer' }} onClick={() => setPasswordVisible(!passwordVisible)} /> : <Eye12Filled style={{ cursor: 'pointer' }} onClick={() => setPasswordVisible(!passwordVisible)} />} />
                    </Field>

                    <p>No account? <span className='login-link' onClick={loading ? undefined : handleRegisterStateSwap} >Create One </span>!</p>

                    {loading && <ProgressBar />}
                    <CardFooter>
                        <Button disabled={loading} appearance='primary' onClick={handleLogin}> Login </Button>
                    </CardFooter>
                </Card>
            </div>
        </FluentProvider>
    );
};

export default LoginPage;