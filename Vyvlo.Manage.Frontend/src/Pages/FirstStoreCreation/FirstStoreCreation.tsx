import { Card } from '@fluentui/react-card'
import { Button, CardFooter, CardHeader, CardPreview, Field, Input, ProgressBar, Textarea, Toaster } from '@fluentui/react-components'
import React, { ChangeEvent, useState } from 'react'
import { ValueError } from '../../Types/ValueError';
import { useToaster } from '../../Contexts/ToasterContext';
import ImagePreview from '../../compnents/ImagePreview/ImagePreview';
import { json } from 'stream/consumers';
import { ErrorOr } from '../../Types/ErrorOr';
import { request } from '../../Types/Agnet';
import { KeyValuePair } from '../../Types/KeyValuePair';
import { useStore } from '../../Contexts/StoreContext';

export const FirstStoreCreationPage = () => {
    const [image, setImage] = useState<ValueError<File | null>>({ error: '', value: null });
    const [description, setDescription] = useState<ValueError<string>>({ error: '', value: '' })
    const [storeName, setStoreName] = useState<ValueError<string>>({ error: '', value: '' })
    const [loading, setLoading] = useState(false);
    const { notify, mainToast } = useToaster();
    const {setStoresData,setCurrentStoreId} =useStore()  
    const handleImageChange = async (e: ChangeEvent<HTMLInputElement>) => {
        await new Promise(resolve => setTimeout(resolve, 100));
        if (e.target.files) {
            setImage({ error: '', value: e.target.files[0] });
        }
    };

    const handleSubmit = async () => {
        let proceed = true;
        if (!storeName.value) {
            setStoreName({ error: 'Store name is required', value: '' });
            proceed = false;
        }
        if (!description.value) {
            setDescription({ error: 'Description is required', value: '' });
            proceed = false;
        }
        if (!image.value) {
            setImage({ error: 'Image is required', value: null });
            proceed = false;
        }
        if (!proceed) {
            return;
        }
        setLoading(true);
        var formData = new FormData();
        formData.append('name', storeName.value);
        formData.append('description', description.value);
        formData.append('storeImage', image.value!);
        try {
            let response = await request.post<ErrorOr<Record<string,string>>>('/api/ManageStore/CreateStoreRequest', formData);
            if (response.isError) {
                notify('Store Creation Failed', 'error');
                response.errors.forEach(e => {
                    if (e.code === 'name') {
                        setStoreName({ error: e.description, value: storeName.value })
                    }else if(e.code === 'description'){
                        setDescription({ error: e.description, value: description.value })
                    }else if(e.code === 'storeImage'){
                        setImage({ error: e.description, value: image.value })
                    }
                });
                return;
            }
            notify('Store Created Successfully', 'success');
            console.log(response.value)
            setCurrentStoreId(Object.keys(response.value)[0])
            setStoresData(response.value)
        } catch (e) {
            notify('Internal Server Error', 'error');
        } finally {
            setLoading(false);
        }
    }

    return (
        <div style={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            height: '100vh',
            background: 'linear-gradient(130deg, rgba(249,249,249,1) 7%, rgba(107,94,182,1) 59%, rgba(130,79,153,1) 91%)',

        }}>
            <Toaster
                toasterId={mainToast}
                position="top-end"
                pauseOnHover
                pauseOnWindowBlur
                timeout={1000}
            />
            <Card
                className='FirstStoreCreationPage-card'>
                <CardHeader header={<h1>Store Creation</h1>} />
                <Field label='Store Name' validationMessage={storeName.error}>
                    <Input placeholder='Enter Store Name...' onChange={(_, d,) => setStoreName({ error: '', value: d.value })} />
                </Field>
                <Field label='Description' validationMessage={description.error} >
                    <Textarea placeholder='Enter a store description' onChange={(_, d,) => setDescription({ error: '', value: d.value })} />
                </Field>
                <div className="file-upload" style={{ display: 'flex', alignItems: 'center' }}>
                    <label
                        htmlFor="file-input"
                        className="file-upload-label"
                        style={{
                            backgroundColor: '#4A5FAE',
                            color: 'white',
                            padding: '10px',
                            borderRadius: '5px',
                            fontFamily: 'Segoe UI',
                            fontSize: '13px',
                            cursor: 'pointer',
                            fontWeight: 'unset'
                        }}
                    >
                        <input
                            type="file"
                            id="file-input"
                            accept="image/*"
                            style={{ display: 'none' }}
                            onChange={handleImageChange}
                        />
                        <span className="file-upload-text"> Upload Images</span>
                    </label>
                    <div>
                        {image.value && (
                            <ImagePreview
                                src={URL.createObjectURL(image.value!)}
                                index={Math.random()}
                                remove={() => setImage({ error: '', value: null })}
                            />
                        )}
                    </div>
                </div>
                {loading && <ProgressBar />}

                <CardFooter>
                    <Button disabled={loading} onClick={handleSubmit} appearance='primary'>Next</Button>
                </CardFooter>
            </Card>
        </div>
    )
}
