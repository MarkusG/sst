import SortableTable, { SortOptions } from "../SortableTable/SortableTable.tsx";
import SortableHeaderCell from "../SortableTable/SortableHeaderCell.tsx";
import { Focus, TransactionTableContext } from "./TransactionTableContext.tsx";
import TransactionRow from "./TransactionRow.tsx";
import { TransactionResponse } from "../Contracts/Responses.ts";
import { useContext } from "react";

export interface TransactionsTableProps {
  options: SortOptions;
  onSortUpdated: (options: SortOptions) => void;
  transactions: TransactionResponse[];
}

export default function TransactionsTable({
  options,
  onSortUpdated,
  transactions,
}: TransactionsTableProps) {
  const [_, setContext] = useContext(TransactionTableContext);

  // invoked when the user navigates out of the last categorization of a transaction
  function moveNext(idx: number) {
    if (idx + 1 >= transactions.length) {
      setContext({});
    } else {
      setContext({
        transactionId: transactions[idx + 1]?.id,
        categorizationId: transactions[idx + 1].categorizations[0]?.id,
        focus: Focus.Category,
      });
    }
  }

  return (
    <SortableTable
      className="w-full table-fixed border-separate border-spacing-0 whitespace-nowrap"
      options={options}
      onSortUpdated={onSortUpdated}
    >
      <thead className="sticky top-0 bg-gray-50 border-b">
        <tr>
          <SortableHeaderCell
            field="timestamp"
            className="w-[160px] px-1 pl-2 border-gray-300 border-r border-b"
          >
            Timestamp
          </SortableHeaderCell>
          <SortableHeaderCell
            field="account"
            className="w-[200px] px-1 border-gray-300 border-r border-b"
          >
            Account
          </SortableHeaderCell>
          <SortableHeaderCell
            field="description"
            className="px-1 border-gray-300 border-r border-b"
          >
            Description
          </SortableHeaderCell>
          <SortableHeaderCell
            field="amount"
            className="w-[100px] px-1 border-gray-300 border-r border-b"
          >
            Amount
          </SortableHeaderCell>
          <td className="w-[200px] px-1 border-gray-300 border-r border-b">
            Category
          </td>
        </tr>
      </thead>
      <tbody>
        {transactions.map((t, idx) => (
          <TransactionRow
            transaction={t}
            onMoveNext={() => moveNext(idx)}
            key={t.id}
          />
        ))}
      </tbody>
    </SortableTable>
  );
}
