import Card from "../Card";
import SortableHeaderCell from "../SortableHeaderCell";
import SortableTable from "../SortableTable/SortableTable";
import TransactionRow from "./TransactionRow";

function TransactionsPage() {
    return (
        <div className="flex flex-col h-screen">
            <div className="px-4 pt-2">
                <h1 className="text-3xl">Transactions</h1>
                <p className="mt-2">Showing 4 out of 12345 transactions</p>
            </div>
            <SortableTable className="w-full table-auto border-collapse whitespace-nowrap">
                <thead>
                    <tr className="border-b border-gray-300">
                        <SortableHeaderCell field="timestamp" className="px-1 pl-4 border-r border-gray-300">Timestamp</SortableHeaderCell>
                        <SortableHeaderCell field="amount" className="px-1 border-r border-gray-300">Amount</SortableHeaderCell>
                        <SortableHeaderCell field="description" className="px-1 border-r border-gray-300">Description</SortableHeaderCell>
                        <SortableHeaderCell field="account" className="px-1 border-r border-gray-300">Account</SortableHeaderCell>
                        <SortableHeaderCell field="category" className="px-1 border-r border-gray-300">Category</SortableHeaderCell>
                        <SortableHeaderCell field="note" className="px-1 border-r border-gray-300">Note</SortableHeaderCell>
                        <SortableHeaderCell field="tags" className="px-1 border-r border-gray-300">Tags</SortableHeaderCell>
                    </tr>
                </thead>
                <tbody>
                    <TransactionRow/>
                    <TransactionRow/>
                    <TransactionRow/>
                    <TransactionRow/>
                    <TransactionRow/>
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
