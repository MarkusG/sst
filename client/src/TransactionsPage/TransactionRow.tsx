import React from "react";
import {TransactionResponse} from "../Contracts/Responses";
import Amount from "../Amount";
import Timestamp from "../Timestamp";
import Categorizer from "./Categorizer.tsx";

interface TransactionRowProps {
    transaction: TransactionResponse,
    isCategorizing: boolean,
    onCategorized: (id: number, moveNext: boolean) => void
}

export default function TransactionRow({transaction, isCategorizing, onCategorized}: TransactionRowProps) {
    if (transaction.categorizations.length === 0) {
        return (
            <tr className="odd:bg-gray-100">
                <td className="px-1 pl-2">
                    <Timestamp ts={transaction.timestamp}/>
                </td>
                <td className="px-1">{transaction.account}</td>
                <td className="px-1">{transaction.description}</td>
                <td className="px-1 text-right"><Amount amount={transaction.amount}/></td>
                <td className="px-1">
                    <Categorizer transaction={transaction} onCategorized={onCategorized} isCategorizing={isCategorizing}/>
                </td>
            </tr>
        );
    } else {
        return transaction.categorizations.map((cz, idx) =>
            <tr className="odd:bg-gray-100">
                <td className="px-1 pl-2">
                    {idx === 0 && <Timestamp ts={transaction.timestamp}/>}
                </td>
                <td className="px-1">{idx === 0 && transaction.account}</td>
                <td className="px-1">{idx === 0 && transaction.description}</td>
                <td className="px-1 text-right"><Amount amount={cz.amount}/></td>
                <td className="px-1">
                    <Categorizer transaction={transaction} categorization={cz} onCategorized={onCategorized} isCategorizing={isCategorizing}/>
                </td>
            </tr>
        );
    }
}
