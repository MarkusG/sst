import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';

import './Styles.css';
import SidebarAccounts from './Sidebar/SidebarAccounts';

function Root() {
    useEffect(() => {
        document.title = 'sst';
    });

    return (
        <div className="flex h-screen">
            <header className="p-2 px-4 mb-4 h-full w-48 bg-white shadow-md">
                <div className="text-3xl flex justify-between items-baseline mb-2">
                    sst <span className="text-base">v0.0.1</span>
                </div>
                <SidebarAccounts/>
            </header>
            <div className="max-w-[90vw] mx-auto">
                <Outlet/>
            </div>
        </div>
    );
}

export default Root;
