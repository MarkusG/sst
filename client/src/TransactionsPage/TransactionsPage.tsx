import SortableHeaderCell from "../SortableHeaderCell";
import SortableTable from "../SortableTable/SortableTable";
import TransactionRow from "./TransactionRow";
import { useQuery } from "@tanstack/react-query";
import LoadingIcon from "../LoadingIcon/LoadingIcon";

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

async function query() : Promise<TransactionsResponse> {
    return await fetch("https://localhost:5001/transactions")
        .then((res) => res.json())
}

function TransactionsPage() {
    const { data, error, isLoading } = useQuery<TransactionsResponse>({
        queryKey: ['transactions'],
        queryFn: query
    });


    if (isLoading) {
        return (
            <div className="mt-16 w-min mx-auto">
                <LoadingIcon/>
            </div>
        );
    }

    if (!!error) {
        return (
            <div className="p-2">
                <p className="text-xl">{error.toString()}</p>
            </div>
        );
    }

    return (
        <div className="flex flex-col h-screen">
            <div className="px-4 pt-2">
                <h1 className="text-3xl">Transactions</h1>
                <p className="mt-2">Showing {data.pageCount} out of {data.totalCount} transactions</p>
            </div>
            <div className="overflow-auto">
                <SortableTable className="w-full table-auto border-separate border-gray-300 whitespace-nowrap">
                    <thead className="sticky top-0 bg-gray-50 border-b">
                        <tr>
                            <SortableHeaderCell field="timestamp" className="px-1 pl-4 border-r border-b">Timestamp</SortableHeaderCell>
                            <SortableHeaderCell field="amount" className="px-1 border-r border-b">Amount</SortableHeaderCell>
                            <SortableHeaderCell field="description" className="px-1 border-r border-b">Description</SortableHeaderCell>
                            <SortableHeaderCell field="account" className="px-1 border-r border-b">Account</SortableHeaderCell>
                            <SortableHeaderCell field="category" className="px-1 border-r border-b">Category</SortableHeaderCell>
                        </tr>
                    </thead>
                    <tbody>
                        {data.transactions.map(t => <TransactionRow transaction={t}/>)}
                    </tbody>
                </SortableTable>
            </div>
            <div className="flex justify-around items-baseline m-4 mt-[max(1rem,_auto)]">
                <button className="bg-white hover:bg-gray-50 transition duration-300 p-2 rounded shadow"><i className="fa-solid fa-chevron-left"></i></button>
                <div><input type="number" value="1" className="w-12 text-center appearance-none border shadow rounded"/> / 10</div>
                <button className="bg-white hover:bg-gray-50 transition duration-300 p-2 rounded shadow"><i className="fa-solid fa-chevron-right"></i></button>
            </div>
        </div>
    );
}

export default TransactionsPage;
