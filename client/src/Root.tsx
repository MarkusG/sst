import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';

import './Styles.css';
import SidebarAccounts from './Sidebar/SidebarAccounts';
import Nav from './Sidebar/Nav';

export default function Root() {
    useEffect(() => {
        document.title = 'sst';
    });

    return (
        <div className="flex h-screen">
            <header className="p-2 px-4 mb-4 h-full bg-white shadow-md">
                <div className="text-3xl flex justify-between items-baseline mb-2">
                    sst <span className="text-base">v0.0.1</span>
                </div>
                <Nav/>
                <SidebarAccounts/>
            </header>
            <div className="grow overflow-hidden">
                <Outlet/>
            </div>
        </div>
    );
}
