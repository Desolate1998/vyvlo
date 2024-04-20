import React, {  useState } from 'react'
import { ValueError } from '../../../Types/ValueError';
import { Dialog, DialogTrigger, DialogSurface, DialogBody, DialogTitle, DialogContent, Field, Input, DialogActions, Button, ProgressBar, Toaster } from '@fluentui/react-components';
import { request } from '../../../Types/Agnet';
import { ErrorOr } from '../../../Types/ErrorOr';
import { useToaster } from '../../../Contexts/ToasterContext';
import { Eye12Regular, Eye12Filled } from '@fluentui/react-icons';
interface IProps {
    open: boolean;
    changeOpen: () => void;
}

export const Register: React.FC<IProps> = ({ changeOpen, open }) => {
    const { notify, mainToast } = useToaster()
    const [details, setDetails] = useState<{
        email: ValueError<string>,
        password: ValueError<string>,
        firstName: ValueError<string>,
        lastName: ValueError<string>
    }>({ email: { value: '', error: '' }, password: { value: '', error: '' }, firstName: { value: '', error: '' }, lastName: { value: '', error: '' } })

    const [passwordVisible, setPasswordVisible] = React.useState(false);
    const [loading, setLoading] = useState(false)

    const handleChange = (field: keyof typeof details) => (e: React.ChangeEvent<HTMLInputElement>) => {
        let error = '';
        switch (field) {
            case 'email':
                const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
                if (!emailRegex.test(e.target.value)) {
                    error = 'Invalid email address';
                }
                break;
            case 'password':
                if (e.target.value.length < 8) {
                    error = 'Password must be at least 8 characters';
                }
                break;
            case 'firstName':
                if (e.target.value.trim().length === 0) {
                    error = 'This field is required';
                }
                break;
            case 'lastName':
                if (e.target.value.trim().length === 0) {
                    error = 'This field is required';
                }
                break;
        }
        setDetails(prev => ({ ...prev, [field]: { value: e.target.value, error: error } }));
    }

    const validateForm = () => {
        let isValid = true;
        let fields = ['email', 'password', 'firstName', 'lastName'];

        fields.forEach(field => {
            if (!details[field as keyof typeof details].value.trim()) {
                setDetails(prev => ({ ...prev, [field]: { value: details[field as keyof typeof details].value, error: 'This field is required' } }));
                isValid = false;
            }
        });

        if(details.password.value.length < 8){
            setDetails(prev => ({ ...prev, ['password']: { value: details.password.value, error: 'Password must be at least 8 characters' } }));
            isValid = false;
        }

        let emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if (!emailRegex.test(details.email.value)) {
            setDetails(prev => ({ ...prev, ['email']: { value: details.email.value, error: 'Invalid email format' } }));
            isValid = false;
        }

        return isValid;
    }

    const handleSubmit = async () => {
        try {
            if(!validateForm()){
                return;
            }
            setLoading(true)
            var response = await request.post<ErrorOr<boolean>>('/api/Authentication/Register', {
                email: details.email.value,
                password: details.password.value,
                firstName: details.firstName.value,
                lastName: details.lastName.value
            });
            
            if(response.isError){
                notify('Registration failed','error')
                response.errors.forEach(error => {
                   setDetails(prev => ({ ...prev, [error.code]: { value: details[error.code as keyof typeof details].value, 
                    error: error.description } }));
                })
            }else{
                notify('Registration successful','success')
                close();
            }

        } catch (error) {
            notify('Registration failed internal server error','error')
            
        }finally{
            setLoading(false)
        }
    }

    const close = () => {
        if (!loading) {
            changeOpen();
            setDetails({ email: { value: '', error: '' }, password: { value: '', error: '' }, firstName: { value: '', error: '' }, lastName: { value: '', error: '' } })
        }
    }

    return (
    <>
    <Toaster
        toasterId={mainToast}
        position="top-end"
        pauseOnHover
        pauseOnWindowBlur
        timeout={1000}
      />
        <Dialog open={open} onOpenChange={close} >
            <DialogSurface >
                <DialogBody >
                    <DialogTitle>Registration</DialogTitle>
                    <DialogContent>
                        <Field label='Email' validationMessage={details.email.error} >
                            <Input disabled={loading} placeholder='Please Enter Email Address...' value={details.email.value} onChange={handleChange('email')} />
                        </Field>
                        <Field label='Password' validationMessage={details.password.error}>
                            <Input
                            type={passwordVisible ? 'text' : 'password'}
                            contentAfter={passwordVisible ? <Eye12Regular style={{ cursor: 'pointer' }} onClick={() => setPasswordVisible(!passwordVisible)} /> : <Eye12Filled style={{ cursor: 'pointer' }} onClick={() => setPasswordVisible(!passwordVisible)} />}
                            disabled={loading} placeholder='Please Enter Password...' value={details.password.value} onChange={handleChange('password')} />
                        </Field>
                        <Field label='First Name' validationMessage={details.firstName.error}>
                            <Input disabled={loading} placeholder='Please Your First Name...' value={details.firstName.value} onChange={handleChange('firstName')} />
                        </Field>

                        <Field label='Last Name' validationMessage={details.lastName.error}>
                            <Input disabled={loading} placeholder='Please Your Last Name...' value={details.lastName.value} onChange={handleChange('lastName')} />
                        </Field>
                        <br />
                        {loading && <ProgressBar />}
                    </DialogContent>
                    <DialogActions>
                        <DialogTrigger disableButtonEnhancement>
                            <Button disabled={loading} appearance="secondary">Close</Button>
                        </DialogTrigger>
                        <Button disabled={loading} appearance="primary" onClick={handleSubmit}>Register</Button>
                    </DialogActions>
                </DialogBody>
            </DialogSurface>
        </Dialog>
     </>
    )
}