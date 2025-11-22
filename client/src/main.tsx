import React, { useEffect } from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter, Route, Routes, useNavigate } from "react-router-dom";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import Root from "./Root.tsx";
import TransactionsPage from "./TransactionsPage/TransactionsPage.tsx";
import CashflowPage from "./CashflowPage/CashflowPage.tsx";
import CategoriesPage from "./CategoriesPage/CategoriesPage.tsx";
import ImportPage from "./ImportPage/ImportPage.tsx";
import AccountsPage from "./AccountsPage/AccountsPage.tsx";
//import { ReactQueryDevtools } from '@tanstack/react-query-devtools/build/lib/devtools';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      networkMode: "always",
    },
  },
});

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement,
);
root.render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <Routes>
          <Route element={<Root />}>
            <Route index element={<IndexRedirector />} />
            <Route path="transactions" element={<TransactionsPage />} />
            <Route path="categories" element={<CategoriesPage />} />
            <Route path="cashflow" element={<CashflowPage />} />
            <Route path="accounts" element={<AccountsPage />} />
            <Route path="import" element={<ImportPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  </React.StrictMode>,
);

function IndexRedirector() {
  const navigate = useNavigate();

  useEffect(() => {
    navigate("transactions");
  }, []);

  return <></>;
}
