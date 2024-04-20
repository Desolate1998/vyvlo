import { Card } from '@fluentui/react-components'
import React from 'react'
import { Link } from 'react-router-dom'
import { useTheme } from '../../Contexts/ThemeContext'
import {
    MenuList,
    MenuItem,
    MenuTrigger,
    MenuPopover,
    Menu,
} from "@fluentui/react-components";
import { BoardFilled, CreditCardToolboxFilled,GroupListRegular  } from '@fluentui/react-icons';

export const Sidebar = () => {
    const { currentTheme } = useTheme()


    return (
        <MenuList>
            <RouteLink name='Dashboard' path='' theme={currentTheme} icon={<BoardFilled />} />
     
            <Menu>
          <MenuTrigger disableButtonEnhancement>
            <MenuItem icon={<CreditCardToolboxFilled />}>Manage Store</MenuItem>
          </MenuTrigger>
          <MenuPopover>
            <MenuList>
                <RouteLink name='Manage Categories' path='Manage-Categories' theme={currentTheme} icon={<GroupListRegular />} />
            </MenuList>
          </MenuPopover>
        </Menu>
        </MenuList>

    )
}

const RouteLink: React.FC<{ name: string, path: string, theme: 'dark' | 'light', icon?: any }> = ({ name, path, theme, icon }) => {
    return (
        <MenuItem icon={icon}>
            <Link
                style={{
                    color: theme == 'dark' ? "white" : "black",
                    textDecoration: "none",
                    display: "flex",
                    gap: "10px",
                    alignContent: "center",

                }}
                to={path}
            >
                {name}
            </Link>
        </MenuItem>
    )
}
