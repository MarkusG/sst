import React from 'react'
import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

import Root from './Root.tsx';
import HomePage from './HomePage.tsx';
import TransactionsPage from './TransactionsPage/TransactionsPage.tsx';
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
                path: '/cash-flow',
                element: <TransactionsPage/>
            },
            {
                path: '/targets',
                element: <TransactionsPage/>
            },
            {
                path: '/transactions',
                element: <TransactionsPage/>
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
