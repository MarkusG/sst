import React, {useState} from "react";
import {TransactionResponse} from "../Contracts/Responses";
import Amount from "../Amount";
import Timestamp from "../Timestamp";
import Categorizer from "./Categorizer.tsx";

interface TransactionRowProps {
    transaction: TransactionResponse,
    isCategorizing: boolean,
    onNavigateOut: (moveNext: boolean) => void
}

export default function TransactionRow({transaction, isCategorizing, onNavigateOut}: TransactionRowProps) {
    const [selectedCategoryIndex, setSelectedCategoryIndex] = useState<number | null>(null);

    function categorized(idx: number, moveNext: boolean) {
        if (!moveNext || selectedCategoryIndex! + 1 >= transaction.categorizations.length) {
            setSelectedCategoryIndex(null);
            onNavigateOut(moveNext);
            return;
        }

        if (selectedCategoryIndex! < transaction.categorizations.length + 1) {
            setSelectedCategoryIndex(idx + 1);
        }
    }

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
                    <Categorizer
                        transaction={transaction}
                        onCategorized={moveNext => categorized(0, moveNext)}
                        selected={selectedCategoryIndex === 0 || isCategorizing}
                        onSelected={() => setSelectedCategoryIndex(0)}/>
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
                    <Categorizer
                        transaction={transaction}
                        categorization={cz}
                        onCategorized={moveNext => categorized(idx, moveNext)}
                        selected={selectedCategoryIndex === idx || (!selectedCategoryIndex && isCategorizing && idx === 0)}
                        onSelected={() => setSelectedCategoryIndex(idx)}/>
                </td>
            </tr>
        );
    }
}
