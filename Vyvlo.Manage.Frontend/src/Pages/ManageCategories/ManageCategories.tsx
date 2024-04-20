import { Button, Card, Table, TableBody, TableCell, TableCellLayout, TableHeader, TableHeaderCell, TableRow, Toaster } from '@fluentui/react-components'
import React, { useEffect, useState } from 'react'
import { ButtonDanger } from '../../compnents/Buttons/ButtonDanger'
import { CreateCategory } from './CreateCategory/CreateCategory'
import { Category } from '../../Types/Category'
import { request } from '../../Types/Agnet'
import { ErrorOr } from '../../Types/ErrorOr'
import { User } from '../../Types/User'
import { useStore } from '../../Contexts/StoreContext'
import { useToaster } from '../../Contexts/ToasterContext'
import { EditCategory } from './EditCategory/EditCategory'

export const ManageCategories = () => {
    const [createCategoryOpen, setCreateCategoryOpen] = useState(false);
    const [editCategoryOpen, setEditCategoryOpen] = useState(false);    
    const [categories, setCategories] = useState<Category[]>([])
    const [loading, setLoading] = useState(false);
    const { currentStoreId } = useStore()
    const { mainToast, notify } = useToaster()
    const [categoryToEdit, setCategoryToEdit] = useState<Category|null>(null);    

    useEffect(() => {
        getAllCategories();
    }, [])  

    const getAllCategories = async () => {
        setLoading(true)
        try {
            let response = await request.get<ErrorOr<Category[]>>(`/api/ManageStore/GetAllCategories?storeId=${currentStoreId}`);
            console.log(response)
            if (response.isError) {
                notify('Failed to get categories', 'error')
            } else {
                setCategories(response.value)
            }
        } catch (error) {
            notify('Internal Server Error', 'error')
        } finally {
            setLoading(false)
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
            
            {(editCategoryOpen && categoryToEdit!==null) && <EditCategory open={editCategoryOpen} setIsOpen={() => setEditCategoryOpen(false)} category={categoryToEdit} />}

            <CreateCategory open={createCategoryOpen} setIsOpen={() => setCreateCategoryOpen(false)} />
            <div style={{ marginBottom: 10 }}>
                <Button onClick={() => setCreateCategoryOpen(true)} appearance='primary'>Add</Button>
            </div>
            <Card><Table arial-label="Default table" >
                <TableHeader>
                    <TableRow>
                        <TableHeaderCell >
                            Name
                        </TableHeaderCell>
                        <TableHeaderCell>
                            Description
                        </TableHeaderCell>
                        <TableHeaderCell>
                            Product Count
                        </TableHeaderCell>
                        <TableHeaderCell>
                            Edit
                        </TableHeaderCell>
                        <TableHeaderCell>
                            Delete
                        </TableHeaderCell>
                    </TableRow>
                </TableHeader>
                <TableBody >
                {
                    categories.map((category) => {
                        return (
                            <TableRow key={category.categoryId}>
                                <TableCell>
                                    {category.name}
                                </TableCell>
                                <TableCell>
                                    {category.description}
                                </TableCell>
                                <TableCell>
                                    {category.metaTags.length}
                                </TableCell>
                                <TableCell>
                                    <Button 
                                    onClick={() => {
                                        setCategoryToEdit(category)
                                        setEditCategoryOpen(true)
                                    }}  
                                    appearance='secondary'>Edit</Button>
                                </TableCell>
                                <TableCell>
                                    <ButtonDanger>Delete</ButtonDanger>
                                </TableCell>
                            </TableRow>
                        )
                    })
                }
                </TableBody>
            </Table>
            </Card>
        </>

    )
}
