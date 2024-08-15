import React, { useState } from "react";
import { Transaction } from "./TransactionsPage";

interface TransactionRowProps {
    transaction: Transaction
    onUpdated: (transaction: Transaction) => Promise<void>
}

function TransactionRow({ transaction, onUpdated }: TransactionRowProps) {
    const [categorizing, setCategorizing] = useState(false);
    const [category, setCategory] = useState(transaction.category ?? undefined);

    const formatter = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

    async function blur(e: React.FocusEvent<HTMLInputElement>) {
        await onUpdated(transaction);
        setCategorizing(false);
    }

    function input(e: React.ChangeEvent<HTMLInputElement>) {
        setCategory(e.target.value.trim());
        transaction.category = e.target.value.trim();
    }

    function keyDown(e: React.KeyboardEvent<HTMLInputElement>) {
        if (e.key === "Enter") {
            setCategorizing(false);
        }
    }

    return (
        <tr className="odd:bg-gray-100">
            <td className="px-1 pl-4">{transaction.timestamp.toLocaleString()}</td>
            <td className={`px-1 text-right ${transaction.amount < 0 ? "text-red-500" : ""}`}>{formatter.format(transaction.amount)}</td>
            <td className="px-1">{transaction.description}</td>
            <td className="px-1">{transaction.account}</td>
            <td className="px-1">
                {!categorizing ? <button className={transaction.category ? "" : "text-gray-400"} onClick={_ => setCategorizing(true)}>{transaction.category ?? "Add category"}</button>
                    : (
                        <input className="bg-[inherit] focus:bg-gray-200 w-full h-full"
                            value={category}
                            autoFocus
                            onKeyDown={keyDown}
                            onInput={input}
                            onBlur={blur}/>
                    )}
            </td>
        </tr>
    );
}

export default TransactionRow;
