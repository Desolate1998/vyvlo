import { Button, Drawer, DrawerBody, DrawerHeader, DrawerHeaderTitle, DrawerFooter, Field, Input, Textarea, InteractionTag, InteractionTagPrimary, InteractionTagSecondary, TagGroup, Toaster, ProgressBar } from '@fluentui/react-components'
import React, { FC, useState } from 'react'
import { Dismiss24Regular } from "@fluentui/react-icons";
import { ValueError } from '../../../Types/ValueError';
import { AddCircleRegular } from '@fluentui/react-icons';
import { request } from '../../../Types/Agnet';
import { ErrorOr } from '../../../Types/ErrorOr';
import { useStore } from '../../../Contexts/StoreContext';
import { useToaster } from '../../../Contexts/ToasterContext';

interface IProps {
    open: boolean
    setIsOpen: (open: boolean) => void
}

export const CreateCategory: FC<IProps> = ({ open, setIsOpen }) => {
    const [name, setName] = useState<ValueError<string>>({ value: '', error: '' })
    const [description, setDescription] = useState<ValueError<string>>({ value: '', error: '' })
    const [tags, setTags] = useState<ValueError<string[]>>({ value: [], error: '' })
    const [tag, setTag] = useState<ValueError<string>>({ value: '', error: '' })
    const [loading, setLoading] = useState(false);

    const { currentStoreId } = useStore()
    const { notify, mainToast } = useToaster()

    const addTag = () => {
        if (!tag.value.trim()) {
            setTag(prev => ({ ...prev, error: 'This field is required' }))
            return
        }
        if (tags.value.includes(tag.value)) {
            setTags(prev => ({ ...prev, error: 'Tag already exists' }))
            return
        }
        setTags(prev => {
            prev.value.push(tag.value)
            return { value: prev.value, error: '' }
        })
        setTag({ value: '', error: '' })
    }

    const removeTag = (tag: string) => {
        setTags(prev => {
            prev.value = prev.value.filter(t => t !== tag)
            return { value: prev.value, error: '' }
        })
    }

    const validContent = () => {
        setDescription(prev => ({ ...prev, error: '' }))
        setName(prev => ({ ...prev, error: '' }))
        setTags(prev => ({ ...prev, error: '' }))
        setTag(prev => ({ ...prev, error: '' }))
        let valid = true;
        if (!name.value.trim()) {
            setName(prev => ({ ...prev, error: 'This field is required' }))
            valid = false
        }
        if (!description.value.trim()) {
            setDescription(prev => ({ ...prev, error: 'This field is required' }))
            valid = false
        }
        if (tags.value.length === 0) {
            setTags(prev => ({ ...prev, error: 'This field is required' }))
            valid = false
        }
        return valid
    }

    const onSubmit = async () => {
        if (!validContent()) return

        setLoading(true)

        try {
            let response = await request.post<ErrorOr<any>>('/api/ManageStore/CreateCategory', {
                name: name.value,
                description: description.value,
                tags: tags.value,
                storeId: currentStoreId
            });
            if(response.isError){
                notify(response.firstError.description, 'error')
            }else{
                close()
            }
        } catch (error) {
            notify('Internal Server Error', 'error')
        } finally {
            setLoading(false)
        }



    }

    const close = () => {
        setIsOpen(false)
        setDescription({ value: '', error: '' })
        setName({ value: '', error: '' })
        setTags({ value: [], error: '' })
        setTag({ value: '', error: '' })
    }



    return (
        <Drawer open={open} position='end'>
            <Toaster
                toasterId={mainToast}
                position="top-end"
                pauseOnHover
                pauseOnWindowBlur
                timeout={1000}
            />
            <DrawerHeader>
                <DrawerHeaderTitle
                    action={
                        <Button
                            disabled={loading}
                            appearance="subtle"
                            aria-label="Close"
                            icon={<Dismiss24Regular />}
                            onClick={() => close()}
                        />
                    }
                >
                    Create Category
                </DrawerHeaderTitle>
                <div>
                    {loading && <ProgressBar/>}
                </div>
            </DrawerHeader>
            <DrawerBody>
                <Field label='Name' validationMessage={name.error} >
                    <Input disabled={loading} placeholder='Enter the category name' value={name.value} onChange={(_, data) => setName({ value: data.value, error: '' })} />
                </Field>
                <Field label='Description' validationMessage={description.error} >
                    <Textarea disabled={loading} placeholder='Enter the category description' value={description.value} onChange={(_, data) => setDescription({ value: data.value, error: '' })} />
                </Field>
                <Field label='Meta Tag' validationMessage={tags.error} hint='These are tags used to describe your category'>
                    <Input
                        disabled={loading}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                addTag()
                            }
                        }}
                        placeholder='Enter a meta tag...' value={tag.value} onChange={(_, data) => setTag({ value: data.value, error: '' })}
                        contentAfter={
                            <Button
                                disabled={loading}
                                onClick={addTag}
                                appearance="transparent"
                                icon={<AddCircleRegular />}
                                size="small"
                            />
                        }
                    />
                </Field>
                <TagGroup aria-label="Dismiss example" style={{ display: 'flex', maxWidth: '100%', flexWrap: 'wrap', justifyContent: 'start' }}>
                    {tags.value.map((tag, index) => (
                        <InteractionTag value={tag} key={tag} style={{ margin: '1px' }}>
                            <InteractionTagPrimary
                                hasSecondaryAction
                            >
                                {tag}
                            </InteractionTagPrimary>
                            <InteractionTagSecondary aria-label="remove" onClick={() => removeTag(tag)} />
                        </InteractionTag>
                    ))}
                </TagGroup>

            </DrawerBody>
            <DrawerFooter>
                <Button appearance="primary" disabled={loading} onClick={onSubmit} >Submit</Button>
                <Button disabled={loading} onClick={close}>Cancel</Button>
            </DrawerFooter>
        </Drawer>
    )
}
