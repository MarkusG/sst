import React from 'react'
import ReactDOM from 'react-dom/client'
import {createBrowserRouter, RouterProvider} from 'react-router-dom';

import {QueryClient, QueryClientProvider} from '@tanstack/react-query';

import Root from './Root.tsx';
import HomePage from './HomePage.tsx';
import TransactionsPage from './TransactionsPage/TransactionsPage.tsx';
import CashflowPage from './CashflowPage/CashflowPage.tsx';
import CategoriesPage from './CategoriesPage/CategoriesPage.tsx';
import ImportPage from "./ImportPage/ImportPage.tsx";
import AccountsPage from "./AccountsPage/AccountsPage.tsx";
//import { ReactQueryDevtools } from '@tanstack/react-query-devtools/build/lib/devtools';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            networkMode: "always"
        }
    }
});

const router = createBrowserRouter([
    {
        path: '/',
        element: <Root/>,
        children: [
            {
                path: '/',
                element: <HomePage/>
            },
            {
                path: '/transactions',
                element: <TransactionsPage/>
            },
            {
                path: '/categories',
                element: <CategoriesPage/>
            },
            {
                path: '/cashflow',
                element: <CashflowPage/>
            },
            {
                path: '/accounts',
                element: <AccountsPage/>
            },
            {
                path: '/import',
                element: <ImportPage/>
            }
        ]
    }
]);

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
root.render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <RouterProvider router={router}/>
        </QueryClientProvider>
    </React.StrictMode>
);
