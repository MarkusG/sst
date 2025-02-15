import {CategoriesResponse, CategorizationResponse, TransactionResponse} from "../Contracts/Responses.ts";
import React, {useState} from "react";
import {useMutation, useQuery, useQueryClient} from "@tanstack/react-query";

export interface CategorizerProps {
    transaction: TransactionResponse,
    categorization?: CategorizationResponse,
    onCategorized: (transactionId: number, moveNext: boolean) => any | Promise<any>,
    isCategorizing: boolean
}

export default function Categorizer({transaction, categorization, onCategorized, isCategorizing}: CategorizerProps) {
    const [categorizing, setCategorizing] = useState(false);
    const [category, setCategory] = useState<string | undefined>(categorization?.category);

    const queryClient = useQueryClient();

    const {data} = useQuery<CategoriesResponse>({
        queryKey: ['categories'],
        queryFn: async () => await fetch('https://localhost:5001/categories')
            .then((res) => res.json())
    });

    const updateMutation = useMutation({
        mutationFn: async (category: string | null) => {
            await fetch(`https://localhost:5001/transactions/${transaction.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({...transaction, category: category})
            });
        },
        onSuccess: () => queryClient.invalidateQueries(['transactions'])
    });

    async function categorize(category: string | null, moveNext: boolean) {
        await updateMutation.mutateAsync(category);
        queryClient.invalidateQueries(['categories']);
        setCategorizing(false);
        onCategorized(transaction.id, moveNext);
    }

    async function finishInput(moveNext: boolean) {
        if (!!category?.trim()) {
            await categorize(category.trim(), moveNext);
        } else {
            await categorize(null, moveNext);
        }
    }

    async function blur() {
        await finishInput(false);
    }

    function input(e: React.ChangeEvent<HTMLInputElement>) {
        setCategory(e.target.value);
    }

    async function keyDown(e: React.KeyboardEvent<HTMLInputElement>) {
        if (e.key === "Enter") {
            await finishInput(true);
        }
    }

    if (!categorizing && !isCategorizing) {
        return (
            <button
                className={`w-full text-left ${categorization ? "" : "text-gray-400"}`}
                onClick={_ => setCategorizing(true)}
                onFocus={_ => setCategorizing(true)}>
                {categorization?.category ?? "Add category"}
            </button>
        );
    }

    return (
        <>
            <input className="bg-[inherit] focus:bg-gray-200 w-full h-full" list="categoryList"
                   value={category ?? undefined}
                   autoFocus
                   onKeyDown={keyDown}
                   onInput={input}
                   onBlur={blur}/>
            {!!data &&
                <datalist id="categoryList">
                    {data.categories.map(c => <option value={c}></option>)}
                </datalist>
            }
        </>
    );
}