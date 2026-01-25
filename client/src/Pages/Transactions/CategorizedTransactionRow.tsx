import TransactionRowProps from "./TransactionRowProps.ts";
import { CategorizationResponse } from "../../Contracts/Responses.ts";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";
import useRecategorize from "./RecategorizeCommand.ts";
import useUncategorize from "./UncategorizeCommand.ts";
import Timestamp from "../../Timestamp.tsx";
import Amount from "../../Amount.tsx";
import Categorizer from "./Categorizer.tsx";

export interface CategorizedTransactionRowProps extends TransactionRowProps {
  categorization: CategorizationResponse;
  showDetails: boolean;
}

export default function CategorizedTransactionRow({
  transaction,
  categorization,
  showDetails,
  onCategoryUpdated,
}: CategorizedTransactionRowProps) {
  const { mutateAsync: recategorize } = useRecategorize();
  const { mutateAsync: uncategorize } = useUncategorize();

  async function categorized(
    category: string | null,
    after: AfterCategorizationAction,
  ) {
    if (category && category !== categorization.category)
      await recategorize({ id: categorization.id, categoryName: category });
    if (!category) await uncategorize({ id: categorization.id });

    onCategoryUpdated(after);
  }

  return (
    <tr className="odd:bg-gray-100">
      <td className="px-1 pl-2">
        {showDetails && <Timestamp ts={transaction.timestamp} />}
      </td>
      <td className="px-1">{showDetails && transaction.account}</td>
      <td className="px-1 overflow-hidden text-ellipsis">
        {showDetails && transaction.description}
      </td>
      <td className="px-1 text-right">
        <Amount amount={categorization.amount} />
      </td>
      <td className="px-1">
        <Categorizer
          transaction={transaction}
          categorization={categorization}
          onCategorized={categorized}
        />
      </td>
    </tr>
  );
}
