import Timestamp from "../Timestamp.tsx";
import Amount from "../Amount.tsx";
import Categorizer from "./Categorizer.tsx";
import TransactionRowProps from "./TransactionRowProps.ts";

export default function UncategorizedTransactionRow({transaction, onCategoryUpdated}: TransactionRowProps) {
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
                    onCategorized={async (_, after) => await onCategoryUpdated(after)}/>
            </td>
        </tr>
    );
}