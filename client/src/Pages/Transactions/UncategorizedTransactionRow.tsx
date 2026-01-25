import Timestamp from "../../Timestamp.tsx";
import Amount from "../../Amount.tsx";
import Categorizer from "./Categorizer.tsx";
import TransactionRowProps from "./TransactionRowProps.ts";
import { useQueryClient } from "@tanstack/react-query";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";
import useCategorize from "./CategorizeCommand.ts";

export default function UncategorizedTransactionRow({
  transaction,
  onCategoryUpdated,
}: TransactionRowProps) {
  useQueryClient();

  const { mutateAsync: categorize } = useCategorize();

  async function categorized(
    category: string | null,
    after: AfterCategorizationAction,
  ) {
    if (category)
      await categorize({
        transactionId: transaction.id,
        categoryName: category,
        amount: transaction.amount,
      });

    onCategoryUpdated(after);
  }

  return (
    <tr className="odd:bg-gray-100">
      <td className="px-1 pl-2">
        <Timestamp ts={transaction.timestamp} />
      </td>
      <td className="px-1">{transaction.account}</td>
      <td className="px-1 overflow-hidden text-ellipsis">
        {transaction.description}
      </td>
      <td className="px-1 text-right">
        <Amount amount={transaction.amount} />
      </td>
      <td className="px-1">
        <Categorizer transaction={transaction} onCategorized={categorized} />
      </td>
    </tr>
  );
}
