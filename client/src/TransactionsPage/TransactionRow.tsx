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
        <tr>
            <td className="px-1 pl-4 border-b">{transaction.timestamp.toLocaleString()}</td>
            <td className={`px-1 text-right border-b ${transaction.amount < 0 ? "text-red-500" : ""}`}>{formatter.format(transaction.amount)}</td>
            <td className="px-1 border-b">{transaction.description}</td>
            <td className="px-1 border-b">{transaction.account}</td>
            <td className="px-1 border-b">{transaction.category}</td>
        </tr>
    );
}

export default TransactionRow;
