import { Breadcrumb, Button, Card, CardHeader, Dropdown, Persona, Option, Menu, MenuItem, MenuList, MenuPopover, MenuTrigger, Switch } from '@fluentui/react-components'
import { AppFolderFilled, AppFolderRegular } from '@fluentui/react-icons';
import React, { useState, useEffect, FC } from 'react'
import { useStore } from '../../Contexts/StoreContext';
import { useTheme } from '../../Contexts/ThemeContext';

const MobileHeader: FC<{ toggle: () => void }> = ({ toggle }) => {
  const { swapTheme, theme, currentTheme } = useTheme()
  return (
    <Card style={{ display: 'grid', gridTemplateColumns: '90% auto', gridColumnGap: '10px' }}>
      <div style={{ padding: '10px' }}>
            <Button icon={<AppFolderFilled />} onClick={toggle} />
      </div>
      <div style={{ padding: '10px' }}>        <Menu>
        <MenuTrigger disableButtonEnhancement>
          <Persona />
        </MenuTrigger>
        <MenuPopover>
          <MenuList>
            <MenuItem>Profile </MenuItem>
            <Switch checked={currentTheme == 'dark'} onChange={() => swapTheme()} />
            <MenuItem>New Window</MenuItem>
          </MenuList>
        </MenuPopover>
      </Menu></div>
    </Card>
  )
}

const NormalHeader:FC<{ toggle: () => void }> = ({ toggle }) => {
  const { stores, currentStoreId } = useStore();

  const { swapTheme, currentTheme } = useTheme()

  return (
    <Card style={{ display: 'grid', gridTemplateColumns: '2% 94% auto', gridColumnGap: '10px' }}>
      <div style={{ padding: '10px' }}>
            <Button icon={<AppFolderFilled />} onClick={toggle} />
      </div>
      <div style={{ padding: '10px' }}>
        <Dropdown value={stores[currentStoreId!]} id="dropdown-id">
          {Object.keys(stores).map((key) => {
            return <Option key={key}>{stores[key]}</Option>
          })}
        </Dropdown>
      </div>
      <div style={{ padding: '10px' }}>
        <Menu>
          <MenuTrigger disableButtonEnhancement>
            <Persona />
          </MenuTrigger>
          <MenuPopover>
            <MenuList>
              <MenuItem>Profile </MenuItem>
              <Switch checked={currentTheme == 'dark'} onChange={() => swapTheme()} />
              <MenuItem>New Window</MenuItem>
            </MenuList>
          </MenuPopover>
        </Menu>
      </div>
    </Card>
  )
}

export const Header: FC<{ toggle: () => void }> = ({ toggle }) => {
  const [isMobile, setIsMobile] = useState(window.innerWidth <= 500);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
    };

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  return isMobile ? <MobileHeader toggle={toggle} /> : <NormalHeader toggle={toggle}/>;
};