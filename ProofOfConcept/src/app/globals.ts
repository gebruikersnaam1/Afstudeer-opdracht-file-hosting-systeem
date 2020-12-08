export interface Fun<A,B>{
    f : (x:A) => B
    then:<C>(g:Fun<B,C>) => Fun<A,C>
}

let then = function<A,B,C>(f:Fun<A,B>, g: Fun<B,C>) : Fun<A,C>{
    return Fun((x:A) =>  g.f(f.f(x)))
}

export let Fun = function<A,B>(f: (x:A) => B) : Fun<A,B>{
    return {
        f: f,
        then: function<C>(this: Fun<A,B>, g:Fun<B,C>) : Fun<A,C>{
            return then(this, g);
        }
    }
}