import { Transaction } from "./TransactionsPage";

interface TransactionRowProps {
    transaction: Transaction
}

function TransactionRow({ transaction }: TransactionRowProps) {
    const formatter = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

    return (
        <tr className="border-b border-gray-300">
            <td className="px-1 pl-4">{transaction.timestamp.toLocaleString()}</td>
            <td className={`px-1 text-right ${transaction.amount < 0 ? "text-red-500" : ""}`}>{formatter.format(transaction.amount)}</td>
            <td className="px-1">{transaction.description}</td>
            <td className="px-1">{transaction.account}</td>
            <td className="px-1">{transaction.category}</td>
        </tr>
    );
}

export default TransactionRow;
