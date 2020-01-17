using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExerciseThree
{
    public class CouponManagerShould
    {
        private ILogger mockILogger;
        private ICouponProvider mockICouponProvider;

        [SetUp]
        public void Setup()
        {
            mockILogger = new Mock<ILogger>().Object;
            mockICouponProvider = new Mock<ICouponProvider>().Object;
        }

        [Test]
        public void Exists()
        {
            var manager = new CouponManager(mockILogger, mockICouponProvider);
            Assert.That(manager, Is.Not.Null);
        }
        [Test]
        public void CreateInstance()
        {
            var manager = new CouponManager(mockILogger, mockICouponProvider);
            Assert.IsInstanceOf<CouponManager>(manager);
        }
        [Test]
        public void FailConstructorNoLogger()
        {
            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"),
              delegate { new CouponManager(null, mockICouponProvider); });

        }

        [Test]
        public void FailConstructorNoCouponProvider()
        {
            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("couponProvider"),
              delegate { new CouponManager(mockILogger, null); });

        }

        [Test]
        public void FailCanRedeemCouponNoEvaluator()
        {
            Assert.ThrowsAsync(Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("evaluators"),
              async delegate
                {
                    var manager = new CouponManager(mockILogger, mockICouponProvider);
                    await manager.CanRedeemCoupon(Guid.NewGuid(), Guid.NewGuid(), null);
                });

        }
        [Test]
        public void FailCanRedeemCouponNullCoupon()
        {  
            Guid couponId = Guid.NewGuid();
            var mockICouponProviderNullRetrieve = new Mock<ICouponProvider>();
            mockICouponProviderNullRetrieve.Setup(x => x.Retrieve(couponId)).ReturnsAsync<ICouponProvider, Coupon>((Coupon)null);

            List<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();

            Assert.ThrowsAsync(Is.TypeOf<KeyNotFoundException>(),
              async delegate
              {
                  var manager = new CouponManager(mockILogger, mockICouponProviderNullRetrieve.Object);
                  await manager.CanRedeemCoupon(couponId, Guid.NewGuid(), evaluators);
              });
        }

        [Test]
        public async Task CanRedeemCouponTrueIfNoEvaluators()
        {
            Guid couponId = Guid.NewGuid();
            var mockICouponProviderHasRetrieve = new Mock<ICouponProvider>();
            mockICouponProviderHasRetrieve.Setup(x => x.Retrieve(couponId)).ReturnsAsync<ICouponProvider, Coupon>(new Coupon());

            List<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();

            var manager = new CouponManager(mockILogger, mockICouponProviderHasRetrieve.Object);
            var result = await manager.CanRedeemCoupon(couponId, Guid.NewGuid(), evaluators);

            Assert.IsTrue(result);
        }

        public async Task CanRedeemCouponFalseIfEvaluatorReturnsFalse()
        {
            Guid couponId = Guid.NewGuid();
            var mockICouponProviderHasRetrieve = new Mock<ICouponProvider>();
            mockICouponProviderHasRetrieve.Setup(x => x.Retrieve(couponId)).ReturnsAsync(new Coupon());

            List<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();
            evaluators.Add(FakeEvaluator);

            var manager = new CouponManager(mockILogger, mockICouponProviderHasRetrieve.Object);
            var result = await manager.CanRedeemCoupon(couponId, Guid.NewGuid(), evaluators);

            Assert.IsFalse(result);
        }


        public bool FakeEvaluator(Coupon coupon, Guid guid)
        {
            return false;
        }
    }
}