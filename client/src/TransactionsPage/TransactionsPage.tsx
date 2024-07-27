import { useEffect, useState } from "react";
import SortableHeaderCell from "../SortableHeaderCell";
import SortableTable from "../SortableTable/SortableTable";
import TransactionRow from "./TransactionRow";

export interface Transaction {
    timestamp: Date,
    amount: number,
    description: string,
    account: string,
    category: string | null
}

interface TransactionsResponse {
    page: number,
    pageCount: number,
    totalPages: number,
    totalCount: number,
    transactions: Transaction[]
}

function TransactionsPage() {
    const [transactions, setTransactions] = useState<TransactionsResponse | null>(null);

    useEffect(() => {
        const fetchData = async () => {
            const t = await fetch("https://localhost:5001/transactions")
                .then((res) => res.json() as Promise<TransactionsResponse>);
            setTransactions(t);
        }

        fetchData().catch(console.error);
    }, []);

    return (
        <div className="flex flex-col h-screen">
            <div className="px-4 pt-2">
                <h1 className="text-3xl">Transactions</h1>
                <p className="mt-2">Showing {transactions?.pageCount} out of {transactions?.totalCount} transactions</p>
            </div>
            <SortableTable className="w-full table-auto border-collapse whitespace-nowrap">
                <thead>
                    <tr className="border-b border-gray-300">
                        <SortableHeaderCell field="timestamp" className="px-1 pl-4 border-r border-gray-300">Timestamp</SortableHeaderCell>
                        <SortableHeaderCell field="amount" className="px-1 border-r border-gray-300">Amount</SortableHeaderCell>
                        <SortableHeaderCell field="description" className="px-1 border-r border-gray-300">Description</SortableHeaderCell>
                        <SortableHeaderCell field="account" className="px-1 border-r border-gray-300">Account</SortableHeaderCell>
                        <SortableHeaderCell field="category" className="px-1 border-r border-gray-300">Category</SortableHeaderCell>
                    </tr>
                </thead>
                <tbody>
                    {transactions?.transactions.map(t => <TransactionRow transaction={t}/>)}
                </tbody>
            </SortableTable>
            <div className="flex justify-around items-baseline m-4 mt-auto">
                <button className="bg-white hover:bg-gray-50 transition duration-300 p-2 rounded shadow"><i className="fa-solid fa-chevron-left"></i></button>
                <div><input type="number" value="1" className="w-12 text-center appearance-none border shadow rounded"/> / 10</div>
                <button className="bg-white hover:bg-gray-50 transition duration-300 p-2 rounded shadow"><i className="fa-solid fa-chevron-right"></i></button>
            </div>
        </div>
    );
}

export default TransactionsPage;
