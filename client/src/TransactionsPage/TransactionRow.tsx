import React, {Fragment, useContext} from "react";
import {TransactionResponse} from "../Contracts/Responses";
import {AfterCategorizationAction} from "./AfterCategorizationAction.ts";
import UncategorizedTransactionRow from "./UncategorizedTransactionRow.tsx";
import CategorizedTransactionRow from "./CategorizedTransactionRow.tsx";
import {Focus, TransactionTableContext} from "./TransactionTableContext.tsx";
import ExtraCategorizationRow from "./ExtraCategorizationRow.tsx";
import {AfterAmountChangedAction} from "./AfterAmountChangedAction.ts";

interface TransactionRowProps {
    transaction: TransactionResponse,
    onMoveNext: () => void
}

export default function TransactionRow({transaction, onMoveNext}: TransactionRowProps) {
    const [context, setContext] = useContext(TransactionTableContext);

    async function categorized(idx: number, after: AfterCategorizationAction) {
        if (after === AfterCategorizationAction.CreateNew) {
            setContext({
                transactionId: context.transactionId,
                addCategoryAfterIdx: idx,
                focus: Focus.Amount
            });
            return;
        }

        if (after === AfterCategorizationAction.Deselect) {
            setContext({});
            return;
        }

        if (idx + 1 >= transaction.categorizations.length) {
            onMoveNext();
            return;
        }

        setContext({...context, addCategoryAfterIdx: undefined, categorizationId: transaction.categorizations[idx + 1].id})
    }

    async function amountUpdated(after: AfterAmountChangedAction) {
        if (after === AfterAmountChangedAction.Deselect) {
            setContext({});
            return;
        }

        setContext({
            ...context,
            focus: Focus.Category
        });
    }

    if (transaction.categorizations.length === 0) {
        return <UncategorizedTransactionRow
            transaction={transaction}
            onCategoryUpdated={async after => await categorized(0, after)}
            onAmountUpdated={amountUpdated}/>
    } else {
        return transaction.categorizations.map((cz, idx) =>
            <Fragment key={cz.id}>
                <CategorizedTransactionRow
                    categorization={cz}
                    showDetails={idx === 0}
                    transaction={transaction}
                    onCategoryUpdated={async after => await categorized(idx, after)}
                    onAmountUpdated={amountUpdated}/>
                {context.transactionId === transaction.id && context.addCategoryAfterIdx === idx &&
                    <ExtraCategorizationRow
                        categorization={cz}
                        index={idx}
                        transaction={transaction}
                        onCategoryUpdated={async after => await categorized(idx + 1, after)}
                        onAmountUpdated={amountUpdated}/>
                }
            </Fragment>
        );
    }
}
