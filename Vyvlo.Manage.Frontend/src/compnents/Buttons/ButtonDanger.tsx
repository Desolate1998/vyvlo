import { Button, makeStyles } from '@fluentui/react-components'
import React, { FC } from 'react'

interface IProps{
    children: React.ReactNode
    icon?:any
}

export const ButtonDanger:FC<IProps> = ({children,icon}) => {
  return (
    <Button icon={icon} appearance='primary' className='button-danager'>
        {children}
    </Button>
  )
}
