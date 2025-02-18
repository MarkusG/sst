import Timestamp from "../Timestamp.tsx";
import Amount from "../Amount.tsx";
import Categorizer from "./Categorizer.tsx";
import TransactionRowProps from "./TransactionRowProps.ts";
import {CategorizationResponse} from "../Contracts/Responses.ts";

export interface CategorizedTransactionRowProps extends TransactionRowProps {
    categorization: CategorizationResponse,
    showDetails: boolean
}

export default function CategorizedTransactionRow({transaction, categorization, showDetails, onCategoryUpdated}: CategorizedTransactionRowProps) {
    return (
        <tr className="odd:bg-gray-100">
            <td className="px-1 pl-2">
                {showDetails && <Timestamp ts={transaction.timestamp}/>}
            </td>
            <td className="px-1">{showDetails && transaction.account}</td>
            <td className="px-1">{showDetails && transaction.description}</td>
            <td className="px-1 text-right"><Amount amount={categorization.amount}/></td>
            <td className="px-1">
                <Categorizer
                    transaction={transaction}
                    categorization={categorization}
                    onCategorized={async (_, after) => await onCategoryUpdated(after)}/>
            </td>
        </tr>
    );
}