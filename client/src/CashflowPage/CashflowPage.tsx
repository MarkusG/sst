import { useQuery } from "@tanstack/react-query";
import { CashFlowResponse } from "../Contracts/Responses";
import LoadingIcon from "../LoadingIcon/LoadingIcon";
import Amount from "../Amount";

function CashflowPage() {
    const { data, error, isError, isLoading } = useQuery<CashFlowResponse>({
        queryKey: ['cashflow'],
        queryFn: async () => await fetch('https://localhost:5001/cashflow?year=2024')
            .then((res) => res.json())
    });

    if (isLoading) {
        return (
            <div className="mt-16 w-min mx-auto">
                <LoadingIcon/>
            </div>
        );
    }

    if (isError) {
        return (
            <div className="p-2">
                <p className="text-xl">{error!.toString()}</p>
            </div>
        );
    }

    return (
        <div className="pt-2 h-full overflow-auto">
            <div className="grid grid-rows-auto grid-cols-[2fr,_repeat(13,_1fr)] text-right">
                    <div className="pl-4 text-left border-b border-gray-300">Category</div>
                    <div className="border-b border-gray-300 px-2">Jan</div>
                    <div className="border-b border-gray-300 px-2">Feb</div>
                    <div className="border-b border-gray-300 px-2">Mar</div>
                    <div className="border-b border-gray-300 px-2">Apr</div>
                    <div className="border-b border-gray-300 px-2">May</div>
                    <div className="border-b border-gray-300 px-2">Jun</div>
                    <div className="border-b border-gray-300 px-2">Jul</div>
                    <div className="border-b border-gray-300 px-2">Aug</div>
                    <div className="border-b border-gray-300 px-2">Sep</div>
                    <div className="border-b border-gray-300 px-2">Oct</div>
                    <div className="border-b border-gray-300 px-2">Nov</div>
                    <div className="border-b border-gray-300 px-2">Dec</div>
                    <div className="border-b border-gray-300 px-4">Total</div>
                    {data.categories.map(c => <>
                        <div className="contents [&_div]:even:bg-gray-100">
                            <div className="text-left pl-4 bg-inherit">{c.category}</div>
                            <div className="px-2"><Amount amount={c.january}/></div>
                            <div className="px-2"><Amount amount={c.february}/></div>
                            <div className="px-2"><Amount amount={c.march}/></div>
                            <div className="px-2"><Amount amount={c.april}/></div>
                            <div className="px-2"><Amount amount={c.may}/></div>
                            <div className="px-2"><Amount amount={c.june}/></div>
                            <div className="px-2"><Amount amount={c.july}/></div>
                            <div className="px-2"><Amount amount={c.august}/></div>
                            <div className="px-2"><Amount amount={c.september}/></div>
                            <div className="px-2"><Amount amount={c.october}/></div>
                            <div className="px-2"><Amount amount={c.november}/></div>
                            <div className="px-2"><Amount amount={c.december}/></div>
                            <div className="px-4 border-l border-gray-300"><Amount amount={c.total}/></div>
                        </div>
                    </>)}
                    <div className="contents [&_div]:even:bg-gray-100">
                        <div className="text-left pl-4 bg-inherit border-t border-gray-300">Total</div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.january}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.february}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.march}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.april}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.may}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.june}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.july}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.august}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.september}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.october}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.november}/></div>
                        <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.december}/></div>
                        <div className="px-4 border-t border-l border-gray-300"><Amount amount={data.totals.total}/></div>
                    </div>
            </div>
        </div>
    )
}

export default CashflowPage;
